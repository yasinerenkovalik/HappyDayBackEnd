#!/bin/sh

# Setup script for HappyDayBackEnd on Alpine Linux
# This script installs all necessary dependencies to build and run the .NET 8 application

set -e  # Exit immediately if a command exits with a non-zero status

echo "Starting setup for HappyDayBackEnd on Alpine Linux..."

# Update package index
echo "Updating package index..."
apk update

# Install basic utilities
echo "Installing basic utilities..."
apk add --no-cache \
    curl \
    wget \
    bash \
    git \
    nano \
    unzip

# Install .NET 8 SDK
echo "Installing .NET 8 SDK..."
apk add --no-cache \
    dotnet8-sdk

# Install PostgreSQL client (useful for database operations)
echo "Installing PostgreSQL client..."
apk add --no-cache \
    postgresql16-client

# Verify installations
echo "Verifying installations..."
dotnet --version
psql --version

# Restore .NET dependencies
echo "Restoring .NET dependencies..."
cd /workspace/HappyDayBackEnd
dotnet restore

# Build the solution
echo "Building the solution..."
dotnet build --no-restore

echo "Setup complete!"
echo ""
echo "To run the application:"
echo "  cd /workspace/HappyDayBackEnd/Presentation/HappyDay.Api"
echo "  dotnet run"
echo ""
echo "To run tests (if any):"
echo "  cd /workspace/HappyDayBackEnd"
echo "  dotnet test"
echo ""
echo "NOTE: You may need to configure your database connection string in appsettings.Development.json"
echo "      before running the application."
