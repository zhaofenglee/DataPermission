$migrationName = Read-Host -Prompt 'Enter migration name'
Write-Host "Adding migration'$migrationName'"
dotnet ef migrations add $migrationName
"Added migration."

Write-Host "Press enter to update the database or CTRL-C to exit ..."
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

dotnet ef database update
"Database update completed."

pause