# Git历史清理脚本
# 用于删除已经提交到Git的不应该存在的文件和目录

Write-Host "开始清理Git历史中的不必要文件..." -ForegroundColor Green

# 检查是否在Git仓库中
if (-not (Test-Path ".git")) {
    Write-Host "错误：当前目录不是Git仓库" -ForegroundColor Red
    exit 1
}

# 备份当前分支
$currentBranch = git branch --show-current
Write-Host "当前分支: $currentBranch" -ForegroundColor Cyan

# 警告用户
Write-Host ""
Write-Host "警告：此操作将重写Git历史！" -ForegroundColor Yellow
Write-Host "建议在执行前备份仓库。" -ForegroundColor Yellow
Write-Host ""
$confirm = Read-Host "是否继续？(y/N)"

if ($confirm -ne "y" -and $confirm -ne "Y") {
    Write-Host "操作已取消" -ForegroundColor Yellow
    exit 0
}

Write-Host ""
Write-Host "开始清理..." -ForegroundColor Green

# 要删除的文件和目录列表
$filesToRemove = @(
    ".vs/"
    "src/.vs/"
    "src/bin/"
    "src/obj/"
    "*.user"
    "src/*.user"
    "bin/"
    "obj/"
)

# 使用git filter-branch删除文件
foreach ($file in $filesToRemove) {
    Write-Host "删除: $file" -ForegroundColor Yellow
    
    # 使用git filter-repo（推荐）或git filter-branch
    # 首先尝试git filter-repo（如果安装了）
    $filterRepoExists = Get-Command git-filter-repo -ErrorAction SilentlyContinue
    
    if ($filterRepoExists) {
        git filter-repo --path $file --invert-paths --force 2>$null
    } else {
        # 回退到git filter-branch
        git filter-branch --force --index-filter "git rm -rf --cached --ignore-unmatch $file" --prune-empty --tag-name-filter cat -- --all 2>$null
    }
}

# 清理引用
Write-Host "清理Git引用..." -ForegroundColor Green
git for-each-ref --format="%(refname)" refs/original/ | ForEach-Object { git update-ref -d $_ }

# 清理reflog
Write-Host "清理reflog..." -ForegroundColor Green
git reflog expire --expire=now --all

# 垃圾回收
Write-Host "执行垃圾回收..." -ForegroundColor Green
git gc --prune=now --aggressive

Write-Host ""
Write-Host "清理完成！" -ForegroundColor Green
Write-Host ""
Write-Host "接下来的步骤：" -ForegroundColor Cyan
Write-Host "1. 检查仓库状态: git status" -ForegroundColor White
Write-Host "2. 如果一切正常，强制推送: git push --force-with-lease origin $currentBranch" -ForegroundColor White
Write-Host "3. 通知团队成员重新克隆仓库" -ForegroundColor White
Write-Host ""
Write-Host "注意：强制推送会重写远程历史，请确保团队成员了解此变更！" -ForegroundColor Yellow
