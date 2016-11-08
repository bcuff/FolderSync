$service = Get-WmiObject -Class Win32_Service -Filter "Name='FolderSync'"
$service.delete()