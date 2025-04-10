# Usando imagen bse de ASP.NET 9.0 para producción
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

# Usando imagen base de SDK de .NET 9.0 para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["DockerDemo03.Web.csproj", "src/DockerDemo03.Web/"]
RUN dotnet restore "src/DockerDemo03.Web/DockerDemo03.Web.csproj"
WORKDIR "/src/src/DockerDemo03.Web"
COPY . .
RUN dotnet build -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Construir la imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "DockerDemo03.Web.dll" ]