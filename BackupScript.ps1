
$serverName = "meoowka\sqlexpress"  
$databaseName = "MicroSystemTechDB"  
$backupPath = "..\\" + (Get-Date -Format "yyyyMMdd_HHmmss") + ".bak"  

# SQL запрос дл€ резервного копировани€
$sqlQuery = "BACKUP DATABASE [$databaseName] TO DISK = N'$backupPath' WITH NOFORMAT, NOINIT, NAME = N'$databaseName-Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10"

# ¬ыполнение SQL запроса
Invoke-Sqlcmd -ServerInstance $serverName -Query $sqlQuery
Write-Host "–езервна€ копи€ базы данных $databaseName успешно создана."


#$taskAction = New-ScheduledTaskAction -Execute "powershell.exe" -Argument "-ExecutionPolicy Bypass -File 'C:\Scripts\BackupScript.ps1'"
#$taskTrigger = New-ScheduledTaskTrigger -Weekly -DaysOfWeek Monday -At "2:00AM"  # ”станавливаем день и врем€
#$taskSettings = New-ScheduledTaskSettingsSet -AllowStartIfOnBatteriesAreLow $true -DontStopIfGoingOnBatteries $true

#Register-ScheduledTask -Action $taskAction -Trigger $taskTrigger -Settings $taskSettings -TaskName "DatabaseBackupTask" -Description "≈женедельное создание резервной копии базы данных"
