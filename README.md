# PaddleOCR V4 Windows Forms 应用程序

这是一个基于 .NET 8 和 Windows Forms 开发的 PaddleOCR V4 文字识别应用程序，使用 OpenVINO.NET 进行推理加速。

## 功能特性

- **模型下载**: 一键下载 PaddleOCR V4 中文模型
- **配置选项**: 
  - 启用/禁用 GPU 加速
  - 启用/禁用文本方向分类
  - 启用/禁用文本检测
  - 启用/禁用文本识别
  - 可调节线程数量
- **图片导入**: 支持多种图片格式 (JPG, PNG, BMP, TIFF)
- **OCR 识别**: 高精度中文文字识别
- **结果显示**: 详细显示识别结果、置信度、坐标和执行时间

## 系统要求

- Windows 10/11
- .NET 8.0 运行时
- 至少 2GB 可用内存
- 支持 OpenVINO 的 CPU/GPU

## 安装和运行

1. 确保已安装 .NET 8.0 运行时
2. 下载并解压应用程序
3. 运行 `PaddleOCRApp.exe`

## 使用说明

1. **下载模型**: 首次使用时，点击"下载模型"按钮下载 PaddleOCR V4 模型
2. **配置设置**: 根据需要调整 GPU 使用、文本处理选项和线程数量
3. **选择图片**: 点击"选择图片"按钮导入要识别的图片
4. **开始识别**: 点击"开始识别"按钮进行 OCR 处理
5. **查看结果**: 在右侧文本框中查看详细的识别结果

## 依赖项

- OpenVINO.CSharp.API.Extensions.PaddleOCR
- OpenVINO.runtime.win
- OpenCvSharp4
- OpenCvSharp4.runtime.win

## 技术特点

- 基于最新的 PaddleOCR V4 模型
- 使用 OpenVINO 进行推理优化
- 支持 CPU 和 GPU 加速
- 异步处理，界面不会冻结
- 详细的性能统计信息

## 注意事项

- 首次下载模型可能需要较长时间，请保持网络连接
- GPU 加速需要兼容的显卡驱动
- 建议使用清晰、分辨率适中的图片以获得最佳识别效果