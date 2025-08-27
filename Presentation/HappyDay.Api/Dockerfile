# 1. Build aşaması
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Çözüm dosyasını ve proje dosyalarını kopyala
COPY HappyDay.sln .
COPY Presentation/HappyDay.Api/*.csproj Presentation/HappyDay.Api/
COPY Core/HappyDay.Application/*.csproj Core/HappyDay.Application/
COPY Core/HappyDay.Domain/*.csproj Core/HappyDay.Domain/
COPY Infrastructure/HappyDay.Persistance/*.csproj Infrastructure/HappyDay.Persistance/
COPY Infrastructure/HappyDay.Infrastructure/*.csproj Infrastructure/HappyDay.Infrastructure/

# NuGet paketlerini geri yükle
RUN dotnet restore

# Tüm projeyi kopyala
COPY . .

# API projesini yayınla
RUN dotnet publish Presentation/HappyDay.Api/HappyDay.Api.csproj -c Release --no-restore -o /app/publish

# 2. Run aşaması
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Uygulamayı doğrudan çalıştır
ENTRYPOINT ["dotnet", "HappyDay.Api.dll"]