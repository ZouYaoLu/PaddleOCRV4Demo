using OpenCvSharp;
using Sdcb.PaddleOCR;
using Sdcb.PaddleOCR.Models;
using Sdcb.PaddleOCR.Models.Online;
using Sdcb.PaddleInference;
using System.Diagnostics;
using System.Text;

namespace PaddleOCRApp
{
    public partial class MainForm : Form
    {
        private PaddleOcrAll? _ocrPredictor;
        private FullOcrModel? _model;
        private string? _currentImagePath;
        private readonly string _modelsDirectory = Sdcb.PaddleOCR.Models.Online.Settings.GlobalModelDirectory;

        // 模型选择相关
        private readonly Dictionary<string, Func<Task<FullOcrModel>>> _availableModels;

        public MainForm()
        {
            // 初始化可用模型字典 - 只保留中文模型
            _availableModels = new Dictionary<string, Func<Task<FullOcrModel>>>
            {
                ["中文V5 (推荐) - 最新移动端优化"] = () => OnlineFullModels.ChineseV5.DownloadAsync(),
                ["中文V3轻量版 - 速度最快"] = () => OnlineFullModels.ChineseV3Slim.DownloadAsync(),
                ["中文V4 - 当前版本"] = () => OnlineFullModels.ChineseV4.DownloadAsync(),
                ["中文V3 - 标准版本"] = () => OnlineFullModels.ChineseV3.DownloadAsync(),
                ["中文服务器V5 - 精度最高"] = () => OnlineFullModels.ChineseServerV5.DownloadAsync(),
                ["中文服务器V4 - 高精度"] = () => OnlineFullModels.ChineseServerV4.DownloadAsync(),
                ["中文服务器V2 - 经典版本"] = () => OnlineFullModels.ChineseServerV2.DownloadAsync(),
                ["中文V2 - 基础版本"] = () => OnlineFullModels.ChineseV2.DownloadAsync()
            };

            InitializeComponent();
            InitializeModelSelection();
        }

        private void InitializeModelSelection()
        {
            // 填充模型选择下拉框
            comboBoxModelSelection.Items.Clear();
            foreach (var modelName in _availableModels.Keys)
            {
                comboBoxModelSelection.Items.Add(modelName);
            }

            // 默认选择中文V5
            comboBoxModelSelection.SelectedIndex = 0;

            // 更新按钮文本
            UpdateDownloadButtonText();
        }

        private async void comboBoxModelSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 模型选择改变时，更新按钮文本和状态
            UpdateDownloadButtonText();

            // 如果当前模型已下载，自动切换到新模型
            var selectedModelName = comboBoxModelSelection.SelectedItem?.ToString();
            if (selectedModelName != null && IsModelDownloaded(selectedModelName))
            {
                try
                {
                    toolStripStatusLabel.Text = "正在切换模型...";
                    await LoadExistingModelAsync();

                    // 如果之前已经初始化过引擎，需要重新初始化
                    if (_ocrPredictor != null)
                    {
                        toolStripStatusLabel.Text = "模型已切换，请重新初始化引擎";
                        _ocrPredictor?.Dispose();
                        _ocrPredictor = null;
                        btnOCR.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    toolStripStatusLabel.Text = $"模型切换失败：{ex.Message}";
                }
            }
            else
            {
                // 模型未下载，清除当前模型
                _model = null;
                if (_ocrPredictor != null)
                {
                    _ocrPredictor?.Dispose();
                    _ocrPredictor = null;
                    btnOCR.Enabled = false;
                    toolStripStatusLabel.Text = "请先下载选中的模型";
                }
            }
        }

        private void UpdateDownloadButtonText()
        {
            var selectedModelName = comboBoxModelSelection.SelectedItem?.ToString();
            if (selectedModelName == null) return;

            // 检查模型是否已下载
            bool isModelDownloaded = IsModelDownloaded(selectedModelName);

            if (isModelDownloaded)
            {
                btnDownloadModels.Text = "重新下载";
                toolStripStatusLabel.Text = $"已选择 {selectedModelName} (已下载)";
            }
            else
            {
                btnDownloadModels.Text = "立即下载";
                toolStripStatusLabel.Text = $"已选择 {selectedModelName} (未下载)";
            }
        }

        private bool IsModelDownloaded(string modelName)
        {
            try
            {
                // 检查模型是否已下载到缓存目录
                // 每个模型包含多个组件：检测模型、识别模型、分类模型
                var modelKeys = GetModelKeys(modelName);

                // 检查所有组件是否都存在
                foreach (var modelKey in modelKeys)
                {
                    var modelPath = Path.Combine(_modelsDirectory, modelKey);
                    if (!Directory.Exists(modelPath))
                    {
                        return false;
                    }

                    // 检查是否有模型文件（.pdmodel 或 .pdiparams）
                    var modelFiles = Directory.GetFiles(modelPath, "*.pdmodel");
                    var paramFiles = Directory.GetFiles(modelPath, "*.pdiparams");

                    // 至少要有参数文件，模型文件可能在某些情况下不存在
                    if (modelFiles.Length == 0 && paramFiles.Length == 0)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string[] GetModelKeys(string modelName)
        {
            // 根据模型名称返回对应的目录名（包含检测、识别、分类模型）
            return modelName switch
            {
                "中文V5 (推荐) - 最新移动端优化" => new[]
                {
                    "PP-OCRv5_mobile_det_infer",
                    "PP-OCRv5_mobile_rec_infer",
                    "ch_ppocr_mobile_v2.0_cls"
                },
                "中文V3轻量版 - 速度最快" => new[]
                {
                    "ch_PP-OCRv3_det_slim",
                    "ch_PP-OCRv3_rec_slim",
                    "ch_ppocr_mobile_slim_v2.0_cls"
                },
                "中文V4 - 当前版本" => new[]
                {
                    "ch_PP-OCRv4_det",
                    "ch_PP-OCRv4_rec",
                    "ch_ppocr_mobile_v2.0_cls"
                },
                "中文V3 - 标准版本" => new[]
                {
                    "ch_PP-OCRv3_det",
                    "ch_PP-OCRv3_rec",
                    "ch_ppocr_mobile_v2.0_cls"
                },
                "中文服务器V5 - 精度最高" => new[]
                {
                    "PP-OCRv5_server_det_infer",
                    "PP-OCRv5_mobile_rec_infer",
                    "ch_ppocr_mobile_v2.0_cls"
                },
                "中文服务器V4 - 高精度" => new[]
                {
                    "detv4_teacher_inference",
                    "ch_PP-OCRv4_rec",
                    "ch_ppocr_mobile_v2.0_cls"
                },
                "中文服务器V2 - 经典版本" => new[]
                {
                    "ch_ppocr_server_v2.0_det",
                    "ch_ppocr_server_v2.0_rec",
                    "ch_ppocr_mobile_v2.0_cls"
                },
                "中文V2 - 基础版本" => new[]
                {
                    "ch_PP-OCRv2_det",
                    "ch_PP-OCRv3_rec",
                    "ch_ppocr_mobile_v2.0_cls"
                },
                _ => new[]
                {
                    "ch_PP-OCRv4_det",
                    "ch_PP-OCRv4_rec",
                    "ch_ppocr_mobile_v2.0_cls"
                }
            };
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            // 创建模型目录
            Directory.CreateDirectory(_modelsDirectory);

            // 检查是否已下载模型
            await CheckModelsExistenceAsync();
        }

        private async Task CheckModelsExistenceAsync()
        {
            // 检查当前选中的模型是否已下载
            var selectedModelName = comboBoxModelSelection.SelectedItem?.ToString();
            if (selectedModelName != null && IsModelDownloaded(selectedModelName))
            {
                toolStripStatusLabel.Text = "检测到已下载的模型，正在加载...";

                try
                {
                    // 尝试加载已下载的模型
                    await LoadExistingModelAsync();
                    toolStripStatusLabel.Text = "模型已就绪";
                }
                catch (Exception ex)
                {
                    toolStripStatusLabel.Text = $"模型加载失败：{ex.Message}";
                    MessageBox.Show($"模型加载失败，请重新下载模型。\n\n错误详情：{ex.Message}",
                                  "模型加载失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                toolStripStatusLabel.Text = "请先下载模型";
            }

            // 更新按钮文本
            UpdateDownloadButtonText();
        }

        private async Task LoadExistingModelAsync()
        {
            // 加载已存在的模型（不重新下载）
            var selectedModelName = comboBoxModelSelection.SelectedItem?.ToString();
            if (selectedModelName != null && _availableModels.ContainsKey(selectedModelName))
            {
                // 检查模型是否真的已下载
                if (IsModelDownloaded(selectedModelName))
                {
                    // 使用缓存的模型，不重新下载
                    _model = await _availableModels[selectedModelName]();
                    toolStripStatusLabel.Text = "模型已加载，请点击'初始化引擎'";
                }
                else
                {
                    toolStripStatusLabel.Text = "模型未完全下载，请重新下载";
                    _model = null;
                }
            }
            else
            {
                toolStripStatusLabel.Text = "请选择有效的模型";
                _model = null;
            }
        }

        private void checkBoxUseGPU_CheckedChanged(object sender, EventArgs e)
        {
            // 临时禁用GPU模式，因为GPU初始化太慢（14秒）
            if (checkBoxUseGPU.Checked)
            {
                MessageBox.Show("检测到GPU初始化时间过长（约14秒），建议使用CPU模式以获得更好的用户体验。\n\n" +
                              "CPU模式平均识别时间：250毫秒\n" +
                              "GPU模式平均识别时间：670毫秒（包含14秒初始化）\n\n" +
                              "将自动切换到CPU模式。",
                              "性能提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                checkBoxUseGPU.Checked = false;
                return;
            }

            // 只更新状态提示，不自动重新初始化
            if (_model != null && _ocrPredictor != null)
            {
                toolStripStatusLabel.Text = "GPU/CPU设置已更改，请点击'初始化引擎'应用设置";
            }
        }

        private async void btnInitializeEngine_Click(object sender, EventArgs e)
        {
            if (_model == null)
            {
                MessageBox.Show("请先下载模型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnInitializeEngine.Enabled = false;
                btnInitializeEngine.Text = "初始化中...";
                toolStripStatusLabel.Text = "正在初始化OCR引擎...";

                // 在UI线程中获取GPU设置
                bool useGpu = checkBoxUseGPU.Checked;

                await Task.Run(() => InitializeOCRFromModel(useGpu));

                // 更新UI状态
                toolStripStatusLabel.Text = "OCR引擎初始化完成";
                btnOCR.Enabled = !string.IsNullOrEmpty(_currentImagePath);

                MessageBox.Show("OCR引擎初始化成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"OCR引擎初始化失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripStatusLabel.Text = "OCR引擎初始化失败";
            }
            finally
            {
                btnInitializeEngine.Enabled = true;
                btnInitializeEngine.Text = "初始化引擎";
            }
        }

        private string BuildDetailedErrorMessage(Exception ex)
        {
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine("OCR识别失败！");
            errorMessage.AppendLine();

            // 根据异常类型提供具体的错误信息和解决方案
            if (ex is FileNotFoundException)
            {
                errorMessage.AppendLine("错误类型：图片文件未找到");
                errorMessage.AppendLine($"错误详情：{ex.Message}");
                errorMessage.AppendLine();
                errorMessage.AppendLine("解决方案：");
                errorMessage.AppendLine("1. 确认图片文件路径正确");
                errorMessage.AppendLine("2. 检查文件是否被移动或删除");
                errorMessage.AppendLine("3. 重新选择图片文件");
            }
            else if (ex is InvalidDataException)
            {
                errorMessage.AppendLine("错误类型：图片数据问题");
                errorMessage.AppendLine($"错误详情：{ex.Message}");
                errorMessage.AppendLine();
                errorMessage.AppendLine("解决方案：");
                errorMessage.AppendLine("1. 确认图片格式支持（JPG、PNG、BMP等）");
                errorMessage.AppendLine("2. 检查图片文件是否损坏");
                errorMessage.AppendLine("3. 尝试使用其他图片");
                errorMessage.AppendLine("4. 如果图片过大，尝试缩小尺寸");
            }
            else if (ex is InvalidOperationException && ex.Message.Contains("OCR引擎运行失败"))
            {
                errorMessage.AppendLine("错误类型：OCR引擎运行失败");
                errorMessage.AppendLine($"错误详情：{ex.InnerException?.Message ?? ex.Message}");
                errorMessage.AppendLine();
                errorMessage.AppendLine("可能的原因：");
                errorMessage.AppendLine("1. GPU内存不足");
                errorMessage.AppendLine("2. 模型文件损坏");
                errorMessage.AppendLine("3. GPU驱动问题");
                errorMessage.AppendLine("4. 系统内存不足");
                errorMessage.AppendLine();
                errorMessage.AppendLine("解决方案：");
                if (checkBoxUseGPU.Checked)
                {
                    errorMessage.AppendLine("1. 尝试取消勾选'启用GPU'使用CPU模式");
                    errorMessage.AppendLine("2. 关闭其他占用GPU的程序");
                    errorMessage.AppendLine("3. 重启程序释放GPU内存");
                }
                else
                {
                    errorMessage.AppendLine("1. 重新下载模型文件");
                    errorMessage.AppendLine("2. 关闭其他占用内存的程序");
                    errorMessage.AppendLine("3. 重启程序");
                }
                errorMessage.AppendLine("4. 尝试使用更小的图片");
            }
            else
            {
                errorMessage.AppendLine("错误类型：未知错误");
                errorMessage.AppendLine($"错误详情：{ex.Message}");
                errorMessage.AppendLine();
                if (ex.InnerException != null)
                {
                    errorMessage.AppendLine($"内部错误：{ex.InnerException.Message}");
                    errorMessage.AppendLine();
                }
                errorMessage.AppendLine("通用解决方案：");
                errorMessage.AppendLine("1. 重启程序");
                errorMessage.AppendLine("2. 重新下载模型");
                errorMessage.AppendLine("3. 尝试使用其他图片");
                if (checkBoxUseGPU.Checked)
                {
                    errorMessage.AppendLine("4. 尝试取消勾选'启用GPU'");
                }
            }

            errorMessage.AppendLine();
            errorMessage.AppendLine("当前配置信息：");
            errorMessage.AppendLine($"- GPU模式：{(checkBoxUseGPU.Checked ? "启用" : "禁用")}");
            errorMessage.AppendLine($"- 允许识别有角度的文字：{(checkBoxAllowRotateDetection.Checked ? "启用" : "禁用")}");
            errorMessage.AppendLine($"- 允许识别旋转角度大于90度的文字：{(checkBoxEnable180Classification.Checked ? "启用" : "禁用")}");
            errorMessage.AppendLine($"- 图片路径：{_currentImagePath}");

            return errorMessage.ToString();
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
                var selectedModelName = comboBoxModelSelection.SelectedItem?.ToString();
                if (selectedModelName != null && IsModelDownloaded(selectedModelName))
                {
                    var result = MessageBox.Show($"模型 {selectedModelName} 已存在，是否重新下载？", "确认",
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                    {
                        toolStripStatusLabel.Text = "取消下载";
                        return;
                    }
                }

                // 下载选中的模型（会下载到用户缓存目录）
                if (selectedModelName != null && _availableModels.ContainsKey(selectedModelName))
                {
                    toolStripStatusLabel.Text = $"正在下载 {selectedModelName}...";
                    _model = await _availableModels[selectedModelName]();
                }
                else
                {
                    // 默认使用中文V5
                    toolStripStatusLabel.Text = "正在下载中文V5模型...";
                    _model = await OnlineFullModels.ChineseV5.DownloadAsync();
                }

                // 模型下载完成，不需要创建标记文件了，因为PaddleSharp会自动管理缓存

                toolStripStatusLabel.Text = "模型下载完成，请点击'初始化引擎'";
                MessageBox.Show($"模型下载完成！\n" +
                              $"注意：模型实际保存在系统缓存目录中\n" +
                              $"标记文件保存在：{_modelsDirectory}\n\n" +
                              $"请点击'初始化引擎'按钮来初始化OCR引擎。", "提示",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 更新按钮文本
                UpdateDownloadButtonText();
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

        private void InitializeOCRFromModel(bool useGpu)
        {
            if (_model != null)
            {
                try
                {
                    // 安全地释放旧的OCR预测器
                    var oldPredictor = _ocrPredictor;
                    _ocrPredictor = null; // 先设为null，防止并发访问

                    // 根据用户选择配置设备
                    Action<PaddleConfig> deviceConfig;
                    string deviceInfo;
                    bool gpuSuccess = false;

                    if (useGpu)
                    {
                        // 用户选择启用GPU，尝试初始化GPU
                        try
                        {
                            deviceConfig = PaddleDevice.Gpu();
                            deviceInfo = " (NVIDIA GPU加速)";
                            gpuSuccess = true;

                            // 不在这里进行GPU测试，直接创建OCR预测器
                            // 如果GPU有问题，会在实际使用时报错
                        }
                        catch (Exception gpuEx)
                        {
                            // GPU初始化失败，显示详细错误信息
                            var errorMessage = $"GPU启用失败！\n\n" +
                                             $"错误详情：{gpuEx.Message}\n\n" +
                                             $"可能的原因：\n" +
                                             $"1. 显卡驱动版本过旧\n" +
                                             $"2. CUDA环境未正确安装（需要CUDA 12.9）\n" +
                                             $"3. cuDNN版本不匹配（需要cuDNN 9.1.0）\n" +
                                             $"4. GPU运行时包与显卡不匹配\n" +
                                             $"   - RTX 3050需要sm86包\n" +
                                             $"   - RTX 40系列需要sm89包\n" +
                                             $"   - RTX 50系列需要sm120包\n\n" +
                                             $"建议：取消勾选'启用GPU'使用CPU模式，或修复GPU环境后重试。";

                            // 抛出异常，让调用者处理
                            throw new InvalidOperationException(errorMessage, gpuEx);

                            // GPU失败，强制使用CPU
                            deviceConfig = PaddleDevice.Mkldnn();
                            deviceInfo = " (CPU - GPU启用失败)";
                            gpuSuccess = false;
                        }
                    }
                    else
                    {
                        // 用户选择使用CPU
                        deviceConfig = PaddleDevice.Mkldnn();
                        deviceInfo = " (CPU - Intel MKL-DNN)";
                        // 不在后台线程中更新UI
                    }

                    // 创建新的OCR预测器
                    _ocrPredictor = new PaddleOcrAll(_model, deviceConfig)
                    {
                        AllowRotateDetection = checkBoxAllowRotateDetection.Checked,
                        Enable180Classification = checkBoxEnable180Classification.Checked,
                    };

                    // 延迟释放旧的预测器
                    if (oldPredictor != null)
                    {
                        Task.Delay(500).ContinueWith(_ =>
                        {
                            try
                            {
                                oldPredictor.Dispose();
                            }
                            catch
                            {
                                // 忽略释放异常
                            }
                        });
                    }

                    // 简单的状态更新（在调用线程中执行）
                    // 初始化成功
                }
                catch (Exception ex)
                {
                    // 重新抛出异常，让调用者处理
                    throw;
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

            // TODO: 添加OCR引擎健康检查

            try
            {
                btnOCR.Enabled = false;
                toolStripProgressBar.Visible = true;
                toolStripProgressBar.Style = ProgressBarStyle.Marquee;
                toolStripStatusLabel.Text = "正在识别...";

                var stopwatch = Stopwatch.StartNew();

                // 详细的错误诊断
                Mat img = null;
                try
                {
                    // 检查图片文件
                    if (!File.Exists(_currentImagePath))
                    {
                        throw new FileNotFoundException($"图片文件不存在：{_currentImagePath}");
                    }

                    var fileInfo = new FileInfo(_currentImagePath);
                    if (fileInfo.Length == 0)
                    {
                        throw new InvalidDataException("图片文件为空");
                    }

                    toolStripStatusLabel.Text = "正在读取图片...";

                    // 使用 OpenCV 读取图片
                    img = Cv2.ImRead(_currentImagePath);
                    if (img == null || img.Empty())
                    {
                        throw new InvalidDataException($"无法读取图片文件，可能格式不支持：{Path.GetExtension(_currentImagePath)}");
                    }

                    toolStripStatusLabel.Text = $"正在识别... (图片尺寸: {img.Width}x{img.Height})";

                    // 检查图片尺寸是否合理
                    if (img.Width > 10000 || img.Height > 10000)
                    {
                        throw new InvalidDataException($"图片尺寸过大：{img.Width}x{img.Height}，建议使用小于10000x10000的图片");
                    }

                    if (img.Width < 10 || img.Height < 10)
                    {
                        throw new InvalidDataException($"图片尺寸过小：{img.Width}x{img.Height}，无法进行OCR识别");
                    }

                    // 执行OCR识别
                    var result = await Task.Run(() =>
                    {
                        try
                        {
                            // 获取当前的OCR预测器引用，避免在执行过程中被替换
                            var currentPredictor = _ocrPredictor;
                            if (currentPredictor == null)
                            {
                                throw new InvalidOperationException("OCR引擎未初始化");
                            }

                            return currentPredictor.Run(img);
                        }
                        catch (ObjectDisposedException)
                        {
                            throw new InvalidOperationException("OCR引擎已被释放，请重新初始化");
                        }
                        catch (Exception ocrEx)
                        {
                            throw new InvalidOperationException($"OCR引擎运行失败：{ocrEx.Message}", ocrEx);
                        }
                    });

                    stopwatch.Stop();

                    // 显示结果
                    DisplayResults(result, stopwatch.ElapsedMilliseconds);

                    toolStripStatusLabel.Text = $"识别完成，用时 {stopwatch.ElapsedMilliseconds} 毫秒";
                }
                finally
                {
                    img?.Dispose();
                }
            }
            catch (Exception ex)
            {
                var errorDetails = BuildDetailedErrorMessage(ex);
                MessageBox.Show(errorDetails, "OCR识别失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
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