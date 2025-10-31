FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Presentation/HappyDay.Api/HappyDay.Api.csproj", "Presentation/HappyDay.Api/"]
COPY ["Core/HappyDay.Application/HappyDay.Application.csproj", "Core/HappyDay.Application/"]
COPY ["Core/HappyDay.Domain/HappyDay.Domain.csproj", "Core/HappyDay.Domain/"]
COPY ["Infrastructure/HappyDay.Persistance/HappyDay.Persistance.csproj", "Infrastructure/HappyDay.Persistance/"]
RUN dotnet restore "Presentation/HappyDay.Api/HappyDay.Api.csproj"
COPY . .
WORKDIR "/src/Presentation/HappyDay.Api"
RUN dotnet build "HappyDay.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HappyDay.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HappyDay.Api.dll"]
