# 简单的Git清理脚本
# 删除当前已跟踪但不应该存在的文件

Write-Host "开始清理Git中已跟踪的不必要文件..." -ForegroundColor Green

# 检查是否在Git仓库中
if (-not (Test-Path ".git")) {
    Write-Host "错误：当前目录不是Git仓库" -ForegroundColor Red
    exit 1
}

# 要删除的文件和目录模式
$patterns = @(
    "src/bin/*"
    "src/obj/*"
    "src/*.user"
    ".vs/*"
    "src/.vs/*"
    "bin/*"
    "obj/*"
    "*.user"
)

Write-Host "检查并删除已跟踪的文件..." -ForegroundColor Yellow

foreach ($pattern in $patterns) {
    Write-Host "检查模式: $pattern" -ForegroundColor Cyan
    
    # 检查是否有匹配的已跟踪文件
    $trackedFiles = git ls-files $pattern 2>$null
    
    if ($trackedFiles) {
        Write-Host "找到已跟踪的文件，正在删除..." -ForegroundColor Yellow
        git rm -r --cached $pattern 2>$null
        Write-Host "已从Git跟踪中删除: $pattern" -ForegroundColor Green
    } else {
        Write-Host "未找到匹配的已跟踪文件: $pattern" -ForegroundColor Gray
    }
}

# 检查状态
Write-Host ""
Write-Host "当前Git状态:" -ForegroundColor Cyan
git status --short

Write-Host ""
Write-Host "清理完成！" -ForegroundColor Green
Write-Host ""
Write-Host "接下来的步骤：" -ForegroundColor Cyan
Write-Host "1. 检查上面的状态输出" -ForegroundColor White
Write-Host "2. 如果看起来正确，提交更改: git add .gitignore && git commit -m '添加.gitignore并清理不必要的文件'" -ForegroundColor White
Write-Host "3. 推送到远程仓库: git push" -ForegroundColor White
Write-Host ""
Write-Host "注意：.gitignore文件已创建，将防止这些文件再次被跟踪" -ForegroundColor Yellow
