; --------------------------------------------------
; WGestures Inno Setup 打包脚本
; --------------------------------------------------

[Setup]
; 软件基本信息
AppName=WGestures
AppVersion=1.0.0
AppPublisher=WGestures Studio
AppSupportURL=https://github.com/WGestures
AppUpdatesURL=https://github.com/WGestures
AppId=com.yingdev.WGestures
AppMutex=com.yingdev.WGestures
; 默认安装到 64位 系统的 Program Files
DefaultDirName={autopf}\WGestures
DefaultGroupName=WGestures
WizardStyle=modern dynamic
; 输出设置
OutputDir=.\bin
OutputBaseFilename=WGestures_Setup
Compression=lzma2/max
SolidCompression=yes

; 权限及安装模式（允许用户选择只为自己安装还是为所有人安装）
PrivilegesRequiredOverridesAllowed=dialog
ArchitecturesAllowed=x64compatible
; "ArchitecturesInstallIn64BitMode=x64compatible" requests that the
; install be done in "64-bit mode" on x64 or Windows 11 on Arm,
; meaning it should use the native 64-bit Program Files directory and
; the 64-bit view of the registry.
ArchitecturesInstallIn64BitMode=x64compatible
ChangesAssociations=yes
ChangesEnvironment=yes
[Languages]
Name: "chinesesimplified"; MessagesFile: "compiler:Languages\ChineseSimplified.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkedonce

[Files]
; --- 核心：主 exe 留在根目录 ---
Source: ".\publish\WGestures.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\publish\UpdateLog.txt"; DestDir: "{app}"; Flags: ignoreversion
 
Source: ".\publish\WGestures.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\publish\WGestures.deps.json"; DestDir: "{app}"; Flags: ignoreversion

; --- 2. 各种附属资源文件夹（留在根目录） ---
Source: ".\publish\defaults\*"; DestDir: "{app}\defaults"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: ".\publish\fr\*"; DestDir: "{app}\fr"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: ".\publish\QuickStartGuide\*"; DestDir: "{app}\QuickStartGuide"; Flags: ignoreversion recursesubdirs createallsubdirs

; --- 3. 核心：将其余的第三方依赖 DLL 过滤并分流到 lib 文件夹 ---
; 排除掉主 dll，把其他的（如 Vanara, NLua, Serilog 等）放进 lib
Source: ".\publish\*.dll";   DestDir: "{app}"; Flags: ignoreversion

[Icons]
; 创建开始菜单和桌面快捷方式，指向根目录的 exe
Name: "{group}\WGestures"; Filename: "{app}\WGestures.exe"
Name: "{autodesktop}\WGestures"; Filename: "{app}\WGestures.exe"; Tasks: desktopicon
; 3. 【新增】在开始菜单中添加“卸载”快捷方式
Name: "{group}\卸载 WGestures"; Filename: "{uninstallexe}"
[Run]
; 安装完成后提示运行
Filename: "{app}\WGestures.exe"; Description: "{cm:LaunchProgram,WGestures}"; Flags: nowait postinstall skipifsilent
