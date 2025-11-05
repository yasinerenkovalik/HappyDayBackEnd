# 1️⃣ Base imaj (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# 2️⃣ Build imaj (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Proje dosyalarını kopyala
COPY ["Presentation/HappyDay.Api/HappyDay.Api.csproj", "Presentation/HappyDay.Api/"]
COPY ["Core/HappyDay.Application/HappyDay.Application.csproj", "Core/HappyDay.Application/"]
COPY ["Core/HappyDay.Domain/HappyDay.Domain.csproj", "Core/HappyDay.Domain/"]
COPY ["Infrastructure/HappyDay.Persistance/HappyDay.Persistance.csproj", "Infrastructure/HappyDay.Persistance/"]

# Bağımlılıkları indir
RUN dotnet restore "Presentation/HappyDay.Api/HappyDay.Api.csproj"

# Geri kalan dosyaları kopyala
COPY . .
WORKDIR "/src/Presentation/HappyDay.Api"

# Projeyi derle
RUN dotnet build "HappyDay.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3️⃣ Publish aşaması
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HappyDay.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4️⃣ Final imaj (çalıştırma)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Coolify ile uyumlu portta çalışması için:
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "HappyDay.Api.dll"]
