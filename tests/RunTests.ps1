
$StatusCodes = @{}
Get-ChildItem -Path ".\*.*Tests" | ForEach-Object {
  $ProjectDirPath = [string]$_
  & dotnet test $ProjectDirPath -l "console;verbosity=detailed"

  $StatusCodes[$ProjectDirPath] = $LASTEXITCODE
}

Write-Host "[UT] Unit test results"
$StatusCodes | Format-List
