Write-Host "Running Database Migration"

$proc = Start-Process -FilePath "dotnet" -ArgumentList "run" -WorkingDirectory "..\src\RapidBlazor.DbMigration\" -NoNewWindow -PassThru
$proc.WaitForExit()

Write-Host "Running Idp"
Start-Process -FilePath "dotnet" -ArgumentList "run" -WorkingDirectory "..\external\Idp\"

Write-Host "Running Api"
Start-Process -FilePath "dotnet" -ArgumentList "run" -WorkingDirectory "..\src\RapidBlazor.Api\"

Write-Host "Running WebUI"
Start-Process -FilePath "dotnet" -ArgumentList "watch run" -WorkingDirectory "..\src\RapidBlazor.WebUI\"