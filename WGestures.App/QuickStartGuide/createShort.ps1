$WshShell = New-Object -COMObject WScript.Shell
$Shortcut = $WshShell.CreateShortcut(".\Wgestures.lnk")
$Shortcut.TargetPath = ".\WGestures.exe"
$Shortcut.Save()