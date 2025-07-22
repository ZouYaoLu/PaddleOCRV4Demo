using OpenCvSharp;
using Sdcb.OpenVINO.PaddleOCR;
using Sdcb.OpenVINO.PaddleOCR.Models;
using Sdcb.OpenVINO.PaddleOCR.Models.Online;
using System.Diagnostics;

namespace PaddleOCRApp
{
    public partial class MainForm : Form
    {
        private PaddleOcrAll? _ocrPredictor;
        private FullOcrModel? _model;
        private string? _currentImagePath;
        private readonly string _modelsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models");

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
            toolStripStatusLabel.Text = "请先下载模型";
        }

        private async void btnDownloadModels_Click(object sender, EventArgs e)
        {
            try
            {
                btnDownloadModels.Enabled = false;
                toolStripProgressBar.Visible = true;
                toolStripProgressBar.Style = ProgressBarStyle.Marquee;
                toolStripStatusLabel.Text = "正在下载模型...";

                // 下载 PaddleOCR V4 模型
                _model = await OnlineFullModels.ChineseV4.DownloadAsync();
                
                toolStripStatusLabel.Text = "模型下载完成";
                MessageBox.Show("模型下载完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // 初始化 OCR 预测器
                InitializeOCRFromModel();
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
                    _ocrPredictor = new PaddleOcrAll(_model)
                    {
                        AllowRotateDetection = true,
                        Enable180Classification = checkBoxUseClassifier.Checked,
                    };
                    
                    toolStripStatusLabel.Text = "OCR 引擎已就绪";
                    btnOCR.Enabled = _currentImagePath != null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"初始化 OCR 引擎失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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