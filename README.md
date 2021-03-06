# Minimal-Cover
This app can be run either as a console app or a web app.

# Console App
To run this app, see the below syntax documentation.

```powershell
MinimalCover.Console:
  Find the minimal cover given a list of functional dependencies

Usage:
  MinimalCover.Console [options] <fds>

Arguments:
  <fds>

Options:
  -i, --input <Json|Text|Yaml>    The input format of functional dependencies
  -f, --file                      Specify whether the functional dependency argument is a file
  --version                       Show version information
  -?, -h, --help                  Show help and usage information
```

# Docker
You can run `RunDocker.ps1` to build a docker image and run the docker image afterwards. This container will run the `src\MinimalCover.UI.WebApi\MinimalCover.UI.WebApi.csproj` project, where you can check out the APIs at `http://localhost:5000/api/minimal-cover`

## References
* https://uisacad5.uis.edu/cgi-bin/mcrem2/database_design_tool.cgi
* https://www.michalbialecki.com/2018/05/25/how-to-make-you-console-app-look-cool/
