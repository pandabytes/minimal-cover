@echo off

dotnet test .\MinimalCover.Core.Xunit -l "console;verbosity=detailed"

timeout 5