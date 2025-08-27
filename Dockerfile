# Use the official .NET 8 SDK image based on Alpine Linux
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env

# Install curl and other dependencies
RUN apk add --no-cache curl git bash

# Set working directory
WORKDIR /app

# Copy everything
COPY . ./

# Restore as distinct layers
RUN dotnet restore

# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app
COPY --from=build-env /app/out .

# Expose port
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "HappyDay.Api.dll"]