# Etapa de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS build
WORKDIR /App

# Copiar código e restaurar dependências
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Etapa de Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2022
WORKDIR /App

# Copiar arquivos do build
COPY --from=build /App/out .

# Comando de entrada
ENTRYPOINT ["dotnet", "VisionaryAnalytics.Worker.dll"]
