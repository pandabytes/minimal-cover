@echo off

dotnet test .\MinimalCover.Xunit.Core -l "console;verbosity=detailed"

timeout 5