@echo off

dotnet test .\MinimalCover.Xunit -l "console;verbosity=detailed"

timeout 5