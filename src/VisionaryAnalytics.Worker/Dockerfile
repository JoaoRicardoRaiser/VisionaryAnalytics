# Build stage - Ubuntu 22.04 com .NET 7 SDK
FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy AS build

WORKDIR /App
COPY . ./

RUN dotnet restore
RUN dotnet publish -c Release -o out

# Runtime stage - Ubuntu 22.04
FROM mcr.microsoft.com/dotnet/aspnet:7.0-jammy

ENV DEBIAN_FRONTEND=noninteractive

# Instalar TODAS as dependencias necessarias
RUN apt-get update && \
    apt-get install -y \
    libgdiplus \
    libgomp1 \
    libopencv-core4.5d \
    libopencv-imgproc4.5d \
    libopencv-imgcodecs4.5d \
    libopencv-videoio4.5d \
    libopencv-highgui4.5d \
    libopencv-objdetect4.5d \
    libtesseract4 \
    libgtk2.0-0 \
    libgdk-pixbuf2.0-0 \
    && apt-get clean && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /App
COPY --from=build /App/out .

# Copiar bibliotecas nativas para o local correto
RUN if [ -d "/App/runtimes/ubuntu.22.04-x64/native" ]; then \
        cp -v /App/runtimes/ubuntu.22.04-x64/native/*.so /usr/local/lib/; \
        chmod +x /usr/local/lib/*.so; \
        ldconfig; \
        echo "=== Verificando dependencias apos instalacao ==="; \
        ldd /usr/local/lib/libOpenCvSharpExtern.so | grep "not found" || echo "Todas as dependencias resolvidas!"; \
    fi

ENV LD_LIBRARY_PATH=/usr/local/lib:$LD_LIBRARY_PATH

ENTRYPOINT ["dotnet", "VisionaryAnalytics.Worker.dll"]