# Etapa 1: Construcción de los assets con Node.js
FROM node:23-slim AS node-build
WORKDIR /client

# Copiar los archivos del cliente (carpeta Client)
COPY ["Client/package*.json", "./"]
COPY ["Client/", "./"]

# Instalar dependencias y compilar los assets
RUN npm install
RUN npm run build

# Etapa 2: Usando imagen base de SDK de .NET 9.0 para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar los archivos del proyecto y el resto del código fuente
COPY ["DockerDemo03.Web.csproj", "src/DockerDemo03.Web/"]
RUN dotnet restore "src/DockerDemo03.Web/DockerDemo03.Web.csproj"
WORKDIR "/src/src/DockerDemo03.Web"
COPY . .

# Construir la aplicación
RUN dotnet build -c Release -o /app/build

# Etapa 3: Publicación de la aplicación
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Copiar los assets generados por Vite desde la etapa de Node.js
COPY --from=node-build /client/wwwroot /app/publish/wwwroot

# Etapa 4: Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 443

# Copiar la aplicación publicada desde la etapa anterior
COPY --from=publish /app/publish .

# Configurar el punto de entrada de la aplicación
ENTRYPOINT [ "dotnet", "DockerDemo03.Web.dll" ]