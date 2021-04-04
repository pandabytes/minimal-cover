# Build docker image
docker build -t minimalcover-webapi -f .\src\MinimalCover.UI.WebApi\Dockerfile .

# Run docker
docker run --rm -p 5000:80 -it minimalcover-webapi