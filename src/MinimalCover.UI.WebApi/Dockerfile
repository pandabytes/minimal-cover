#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/MinimalCover.UI.WebApi/MinimalCover.UI.WebApi.csproj", "src/MinimalCover.UI.WebApi/"]
COPY ["src/MinimalCover.Infrastructure/MinimalCover.Infrastructure.csproj", "src/MinimalCover.Infrastructure/"]
COPY ["src/MinimalCover.Domain/MinimalCover.Domain.csproj", "src/MinimalCover.Domain/"]
COPY ["src/MinimalCover.Application/MinimalCover.Application.csproj", "src/MinimalCover.Application/"]
RUN dotnet restore "src/MinimalCover.UI.WebApi/MinimalCover.UI.WebApi.csproj"
COPY . .
WORKDIR "/src/src/MinimalCover.UI.WebApi"
RUN dotnet build "MinimalCover.UI.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MinimalCover.UI.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinimalCover.UI.WebApi.dll"]