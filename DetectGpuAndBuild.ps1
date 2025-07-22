# GPU检测和自动编译脚本
param(
    [string]$ProjectPath = "src/PaddleOCRApp.csproj",
    [switch]$Force = $false
)

Write-Host "正在检测NVIDIA GPU..." -ForegroundColor Green

# 检测NVIDIA GPU
$gpuInfo = Get-WmiObject -Class Win32_VideoController | Where-Object { $_.Name -like "*NVIDIA*" }

if (-not $gpuInfo) {
    Write-Host "未检测到NVIDIA GPU，使用CPU运行时编译" -ForegroundColor Yellow
    dotnet build $ProjectPath
    exit
}

$gpuName = $gpuInfo.Name
Write-Host "检测到GPU: $gpuName" -ForegroundColor Cyan

# 根据GPU型号确定架构
$enableProperty = ""
$archDescription = ""

if ($gpuName -match "RTX\s*50\d{2}") {
    $enableProperty = "EnableGpuSM120=true"
    $archDescription = "RTX 50系列 (SM120)"
}
elseif ($gpuName -match "RTX\s*40\d{2}") {
    $enableProperty = "EnableGpuSM89=true"
    $archDescription = "RTX 40系列 (SM89)"
}
elseif ($gpuName -match "RTX\s*30\d{2}") {
    $enableProperty = "EnableGpuSM86=true"
    $archDescription = "RTX 30系列 (SM86)"
}
elseif ($gpuName -match "RTX\s*20\d{2}" -or $gpuName -match "GTX\s*16\d{2}") {
    $enableProperty = "EnableGpuSM75=true"
    $archDescription = "RTX 20系列/GTX 16xx系列 (SM75)"
}
elseif ($gpuName -match "GTX\s*10\d{2}") {
    $enableProperty = "EnableGpuSM61=true"
    $archDescription = "GTX 10系列 (SM61)"
}
else {
    Write-Host "GPU架构未知，使用CPU运行时编译" -ForegroundColor Yellow
    dotnet build $ProjectPath
    exit
}

Write-Host "GPU架构: $archDescription" -ForegroundColor Green
Write-Host "编译属性: $enableProperty" -ForegroundColor Green

# 编译项目
Write-Host "正在编译项目..." -ForegroundColor Green
$buildCommand = "dotnet build `"$ProjectPath`" -p:$enableProperty"

if ($Force) {
    $buildCommand += " --force"
}

Write-Host "执行命令: $buildCommand" -ForegroundColor Gray
Invoke-Expression $buildCommand

if ($LASTEXITCODE -eq 0) {
    Write-Host "编译成功！GPU运行时已包含。" -ForegroundColor Green
    Write-Host ""
    Write-Host "注意事项:" -ForegroundColor Yellow
    Write-Host "- 确保已安装CUDA 12.9和cuDNN 9.1.0" -ForegroundColor White
    Write-Host "- 确保NVIDIA驱动程序是最新版本" -ForegroundColor White
    Write-Host "- 如果GPU加速不工作，程序会自动回退到CPU模式" -ForegroundColor White
} else {
    Write-Host "编译失败！" -ForegroundColor Red
    exit 1
}
