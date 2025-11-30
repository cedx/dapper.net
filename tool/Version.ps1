"Updating the version number in the sources..."
$version = (Import-PowerShellDataFile "Dapper.psd1").ModuleVersion
(Get-Content "src/Client.cs") -replace 'Version = "\d+(\.\d+){2}"', "Version = ""$version""" | Out-File "src/Client.cs"
foreach ($item in Get-Item "*/*.csproj") {
	(Get-Content $item) -replace "<Version>\d+(\.\d+){2}</Version>", "<Version>$version</Version>" | Out-File $item
}
