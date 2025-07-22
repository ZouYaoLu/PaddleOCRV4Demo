# PaddleSharp GPU包使用说明

## 概述
项目已经迁移到PaddleSharp，支持NVIDIA GPU加速。你可以根据自己的显卡型号按需添加相应的GPU运行时包。

## 显卡型号对应的包

### GTX 10系列
```xml
<PackageReference Include="Sdcb.PaddleInference.runtime.win64.cu129_cudnn910_sm61" Version="3.1.0.54" />
```
支持的显卡：GTX 1050, 1060, 1070, 1080, 1080 Ti等

### RTX 20系列/GTX 16xx系列  
```xml
<PackageReference Include="Sdcb.PaddleInference.runtime.win64.cu129_cudnn910_sm75" Version="3.1.0.54" />
```
支持的显卡：RTX 2060, 2070, 2080, 2080 Ti, GTX 1650, 1660, 1660 Ti等

### RTX 30系列
```xml
<PackageReference Include="Sdcb.PaddleInference.runtime.win64.cu129_cudnn910_sm86" Version="3.1.0.54" />
```
支持的显卡：RTX 3060, 3070, 3080, 3090, 3060 Ti, 3070 Ti, 3080 Ti, 3090 Ti等

### RTX 40系列
```xml
<PackageReference Include="Sdcb.PaddleInference.runtime.win64.cu129_cudnn910_sm89" Version="3.1.0.54" />
```
支持的显卡：RTX 4060, 4070, 4080, 4090, 4060 Ti, 4070 Ti, 4080 Super, 4070 Ti Super等

### RTX 50系列
```xml
<PackageReference Include="Sdcb.PaddleInference.runtime.win64.cu129_cudnn910_sm120" Version="3.1.0.54" />
```
支持的显卡：RTX 5060, 5070, 5080, 5090等（最新系列）

## 使用步骤

### 1. 确定你的显卡型号
- 右键"此电脑" → "属性" → "设备管理器" → "显示适配器"
- 或者运行 `nvidia-smi` 命令查看

### 2. 编辑项目文件
打开 `src/PaddleOCRApp.csproj` 文件，找到对应你显卡的包，取消注释：

例如，如果你有RTX 3080（RTX 30系列），取消注释这行：
```xml
<!-- RTX 30系列 -->
<PackageReference Include="Sdcb.PaddleInference.runtime.win64.cu129_cudnn910_sm86" Version="3.1.0.54" />
```

改为：
```xml
<!-- RTX 30系列 -->
<PackageReference Include="Sdcb.PaddleInference.runtime.win64.cu129_cudnn910_sm86" Version="3.1.0.54" />
```

### 3. 安装CUDA环境（如果还没有）
- 下载并安装 [CUDA 12.9](https://developer.nvidia.com/cuda-downloads)
- 下载并安装 [cuDNN 9.1.0](https://developer.nvidia.com/cudnn)
- 确保NVIDIA驱动是最新版本

### 4. 编译项目
```bash
dotnet build src/PaddleOCRApp.csproj
```

### 5. 运行测试
启动应用程序，如果GPU配置正确，程序会显示"成功启用GPU加速！"的消息。

## 注意事项

1. **只添加一个GPU包**：只取消注释对应你显卡的那一个包，不要同时启用多个GPU包。

2. **CPU回退**：如果GPU不可用，程序会自动回退到CPU模式，不会影响基本功能。

3. **CUDA版本**：所有GPU包都基于CUDA 12.9，确保你安装的CUDA版本匹配。

4. **内存要求**：GPU加速需要足够的显存，建议至少4GB显存。

## 故障排除

### GPU加速不工作
1. 检查CUDA和cuDNN是否正确安装
2. 确认添加了正确的GPU包
3. 检查NVIDIA驱动版本
4. 查看错误消息中的具体原因

### 编译错误
1. 确保只启用了一个GPU包
2. 检查包版本是否正确
3. 清理并重新编译：
   ```bash
   dotnet clean src/PaddleOCRApp.csproj
   dotnet build src/PaddleOCRApp.csproj
   ```

### 性能对比
- CPU模式：适合偶尔使用，无需额外配置
- GPU模式：识别速度可提升3-10倍，适合大量OCR任务

## 模型下载改进

项目已经改进了模型下载逻辑：
- 自动检测已下载的模型，避免重复下载
- 模型保存在程序根目录的 `models` 文件夹
- 支持重新下载功能

## 快速开始

如果你有RTX 30系列显卡（最常见），可以直接：

1. 编辑 `src/PaddleOCRApp.csproj`，取消注释RTX 30系列的包
2. 安装CUDA 12.9和cuDNN 9.1.0
3. 编译运行

这样就能享受GPU加速的OCR识别了！
