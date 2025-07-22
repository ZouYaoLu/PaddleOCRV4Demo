using OpenCvSharp;
using Sdcb.PaddleOCR;
using Sdcb.PaddleOCR.Models;
using Sdcb.PaddleOCR.Models.Online;
using Sdcb.PaddleInference;
using System.Diagnostics;

namespace PaddleOCRApp
{
    public partial class MainForm : Form
    {
        private PaddleOcrAll? _ocrPredictor;
        private FullOcrModel? _model;
        private string? _currentImagePath;
        private readonly string _modelsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models");
        private readonly string _modelFileName = "paddleocr_v4_chinese.model";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 创建模型目录
            Directory.CreateDirectory(_modelsDirectory);
            
            // 检查是否已下载模型
            CheckModelsExistence();
        }

        private void CheckModelsExistence()
        {
            string modelPath = Path.Combine(_modelsDirectory, _modelFileName);
            if (File.Exists(modelPath))
            {
                toolStripStatusLabel.Text = "检测到已下载的模型";
                btnDownloadModels.Text = "重新下载模型";
            }
            else
            {
                toolStripStatusLabel.Text = "请先下载模型";
                btnDownloadModels.Text = "下载模型";
            }
        }



        private async void btnDownloadModels_Click(object sender, EventArgs e)
        {
            try
            {
                btnDownloadModels.Enabled = false;
                toolStripProgressBar.Visible = true;
                toolStripProgressBar.Style = ProgressBarStyle.Marquee;
                toolStripStatusLabel.Text = "正在下载模型...";

                // 检查是否需要重新下载
                string modelPath = Path.Combine(_modelsDirectory, _modelFileName);
                bool forceDownload = File.Exists(modelPath);

                if (forceDownload)
                {
                    var result = MessageBox.Show("模型已存在，是否重新下载？", "确认",
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                    {
                        toolStripStatusLabel.Text = "取消下载";
                        return;
                    }
                }

                // 下载 PaddleOCR V4 模型（会下载到用户缓存目录）
                _model = await OnlineFullModels.ChineseV4.DownloadAsync();

                // 创建标记文件到我们的模型目录
                await File.WriteAllTextAsync(modelPath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                toolStripStatusLabel.Text = "模型下载完成";
                MessageBox.Show($"模型下载完成！\n" +
                              $"注意：模型实际保存在系统缓存目录中\n" +
                              $"标记文件保存在：{_modelsDirectory}", "提示",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 初始化 OCR 预测器
                InitializeOCRFromModel();
                btnDownloadModels.Text = "重新下载模型";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"模型下载失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripStatusLabel.Text = "模型下载失败";
            }
            finally
            {
                btnDownloadModels.Enabled = true;
                toolStripProgressBar.Visible = false;
                toolStripProgressBar.Style = ProgressBarStyle.Blocks;
            }
        }

        private void InitializeOCRFromModel()
        {
            if (_model != null)
            {
                try
                {
                    _ocrPredictor?.Dispose();

                    // 尝试使用GPU设备，如果失败则回退到CPU
                    Action<PaddleConfig> deviceConfig;
                    string deviceInfo;

                    try
                    {
                        deviceConfig = PaddleDevice.Gpu();
                        deviceInfo = " (NVIDIA GPU加速)";
                        toolStripStatusLabel.Text = "正在初始化 OCR 引擎 (GPU)...";

                        MessageBox.Show("成功启用GPU加速！", "GPU加速", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception gpuEx)
                    {
                        deviceConfig = PaddleDevice.Mkldnn();
                        deviceInfo = " (CPU - Intel MKL-DNN)";
                        toolStripStatusLabel.Text = "正在初始化 OCR 引擎 (CPU)...";

                        MessageBox.Show($"GPU加速不可用，使用CPU模式。\n\n" +
                                      $"如需GPU加速，请：\n" +
                                      $"1. 在项目文件中取消注释对应显卡的GPU包\n" +
                                      $"2. 安装CUDA 12.9和cuDNN 9.1.0\n" +
                                      $"3. 确保NVIDIA驱动是最新版本\n" +
                                      $"4. 重新编译项目\n\n" +
                                      $"错误详情: {gpuEx.Message}",
                                      "使用CPU模式", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    _ocrPredictor = new PaddleOcrAll(_model, deviceConfig)
                    {
                        AllowRotateDetection = true,
                        Enable180Classification = true, // checkBoxUseClassifier.Checked,
                    };

                    toolStripStatusLabel.Text = $"OCR 引擎已就绪{deviceInfo}";
                    btnOCR.Enabled = _currentImagePath != null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"初始化 OCR 引擎失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    toolStripStatusLabel.Text = "OCR 引擎初始化失败";
                }
            }
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.bmp;*.tiff|所有文件|*.*";
            openFileDialog.Title = "选择要识别的图片";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _currentImagePath = openFileDialog.FileName;
                
                try
                {
                    // 加载并显示图片
                    using var originalImage = new Bitmap(_currentImagePath);
                    pictureBoxImage.Image = new Bitmap(originalImage);
                    
                    // 清空之前的结果
                    textBoxResult.Clear();
                    labelExecutionTime.Text = "执行时间：-";
                    
                    // 启用识别按钮
                    if (_ocrPredictor != null)
                    {
                        btnOCR.Enabled = true;
                    }
                    
                    toolStripStatusLabel.Text = $"已加载图片：{Path.GetFileName(_currentImagePath)}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加载图片失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnOCR_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentImagePath))
            {
                MessageBox.Show("请先选择图片", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_ocrPredictor == null)
            {
                MessageBox.Show("OCR 引擎未初始化，请先下载模型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnOCR.Enabled = false;
                toolStripProgressBar.Visible = true;
                toolStripProgressBar.Style = ProgressBarStyle.Marquee;
                toolStripStatusLabel.Text = "正在识别...";

                var stopwatch = Stopwatch.StartNew();
                
                // 使用 OpenCV 读取图片
                var img = Cv2.ImRead(_currentImagePath);
                var result = await Task.Run(() => _ocrPredictor.Run(img));
                img.Dispose();

                stopwatch.Stop();

                // 显示结果
                DisplayResults(result, stopwatch.ElapsedMilliseconds);
                
                toolStripStatusLabel.Text = $"识别完成，用时 {stopwatch.ElapsedMilliseconds} 毫秒";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"OCR 识别失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripStatusLabel.Text = "识别失败";
            }
            finally
            {
                btnOCR.Enabled = true;
                toolStripProgressBar.Visible = false;
                toolStripProgressBar.Style = ProgressBarStyle.Blocks;
            }
        }

        private void DisplayResults(PaddleOcrResult result, long elapsedMilliseconds)
        {
            textBoxResult.Clear();
            
            if (result == null || string.IsNullOrEmpty(result.Text))
            {
                textBoxResult.Text = "未识别到文字内容";
                labelExecutionTime.Text = $"执行时间：{elapsedMilliseconds} 毫秒";
                return;
            }

            var resultText = new System.Text.StringBuilder();
            
            try
            {
                var regions = result.Regions?.ToList();
                if (regions != null && regions.Count > 0)
                {
                    resultText.AppendLine($"识别到 {regions.Count} 个文本区域：");
                    resultText.AppendLine();

                    for (int i = 0; i < regions.Count; i++)
                    {
                        var region = regions[i];
                        resultText.AppendLine($"[{i + 1}] 文本：{region.Text}");
                        resultText.AppendLine($"    置信度：{region.Score:F4}");
                        
                        try
                        {
                            resultText.AppendLine($"    中心点：({region.Rect.Center.X:F1}, {region.Rect.Center.Y:F1})");
                            resultText.AppendLine($"    尺寸：{region.Rect.Size.Width:F1} x {region.Rect.Size.Height:F1}");
                            resultText.AppendLine($"    角度：{region.Rect.Angle:F1}°");
                        }
                        catch
                        {
                            resultText.AppendLine("    位置信息不可用");
                        }
                        
                        resultText.AppendLine();
                    }
                }

                // 添加汇总信息
                resultText.AppendLine("=== 完整文本 ===");
                resultText.AppendLine(result.Text);
            }
            catch (Exception ex)
            {
                resultText.AppendLine($"处理识别结果时出错：{ex.Message}");
                resultText.AppendLine();
                resultText.AppendLine("=== 原始文本 ===");
                resultText.AppendLine(result.Text ?? "无");
            }

            textBoxResult.Text = resultText.ToString();
            labelExecutionTime.Text = $"执行时间：{elapsedMilliseconds} 毫秒";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // 清理资源
            _ocrPredictor?.Dispose();
            pictureBoxImage.Image?.Dispose();
            
            base.OnFormClosing(e);
        }
    }
}