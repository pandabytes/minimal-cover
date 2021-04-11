# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# Build this Dockerfile at the repo's root level

####################################################################
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /minimalcover
COPY src src

WORKDIR "/minimalcover/src/MinimalCover.UI.WebApi"
RUN dotnet build "MinimalCover.UI.WebApi.csproj" -c Release -o /app/build

####################################################################
FROM build AS publish
RUN dotnet publish "MinimalCover.UI.WebApi.csproj" -c Release -o /app/publish

####################################################################
# FROM base AS final
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinimalCover.UI.WebApi.dll"]
