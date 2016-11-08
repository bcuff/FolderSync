$exe = Resolve-Path ".\FolderSync.exe"
New-Service -Name "FolderSync" -DisplayName "Folder Sync" -BinaryPathName $exe -StartupType Automatic
Start-Service -Name "FolderSync"