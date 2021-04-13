param (
  [Parameter(Mandatory=$false)][Switch] $Prune
)

$ErrorActionPreference = "Stop"

$Tag = "v1.0.0"
$ImageName = "minimalcover-webapi:$Tag"

if ($Prune -eq $True)
{
  & docker system prune -af
}

# Remove existing image 
& docker rmi $ImageName

# Build docker image
& docker build -t $ImageName -f .\src\MinimalCover.UI.WebApi\Dockerfile .

# Run docker
& docker run --rm -p 5000:80 -it $ImageName