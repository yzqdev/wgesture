 
function Log-Info ($Message) {
    Write-Host "[$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] [INFO] $Message" -ForegroundColor Cyan
}

function Log-Success ($Message) {
    Write-Host "[$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] [SUCCESS] $Message" -ForegroundColor Green
}

function Log-Error ($Message) {
    Write-Host "[$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] [ERROR] $Message" -ForegroundColor Red
}

# ----------------------------------------------------
# 脚本主逻辑
# ----------------------------------------------------
$PublishDir = "./publish"

Log-Info "=================================================="
Log-Info "开始执行 .NET 程序发布流程..."
Log-Info "=================================================="

# 1. 执行 dotnet publish
Log-Info "正在编译并发布项目 (Release 模式, 单文件, 依赖框架)..."
dotnet publish -c Release -r win-x64 -o $PublishDir  --self-contained false   

# 检查上一步命令的退出状态码 ($?)
if ($LASTEXITCODE -ne 0) {
    Log-Error "dotnet publish 执行失败！流程终止。"
    exit $LASTEXITCODE
}
Log-Success "项目发布成功，输出目录: $PublishDir"

# 2. 清理 PDB 调试文件
Log-Info "开始清理发布目录中的 .pdb 调试文件..."

# 检查目录下是否存在 PDB 文件
$PdbFiles = Get-ChildItem -Path $PublishDir -Filter *.pdb -ErrorAction SilentlyContinue

if ($PdbFiles) {
    Log-Info "发现 $($PdbFiles.Count) 个 PDB 文件，正在删除..."
    foreach ($file in $PdbFiles) {
        Log-Info "正在删除: $($file.Name)"
    }

    # 执行删除
    Remove-Item -Path "$PublishDir/*.pdb" -Force
    Log-Success "PDB 文件清理完毕。"
} else {
    Log-Info "未在目录中发现 .pdb 文件，跳过清理步骤。"
}
ISCC innoInstall.iss
Log-Info "=================================================="
Log-Success "整个发布流程圆满完成！"
Log-Info "=================================================="