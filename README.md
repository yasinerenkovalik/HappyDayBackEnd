# HappyDayBackEnd Setup for Alpine Linux

This document explains how to set up and run the HappyDayBackEnd application on Alpine Linux.

## Prerequisites

- Alpine Linux OS
- Internet connection

## Setup Instructions

### Option 1: Using the setup script (recommended)

1. Make the setup script executable:
   ```bash
   chmod +x setup-alpine.sh
   ```

2. Run the setup script:
   ```bash
   ./setup-alpine.sh
   ```

This will:
- Install all required dependencies
- Restore .NET packages
- Build the solution

### Option 2: Manual setup

1. Install required packages:
   ```bash
   apk update
   apk add --no-cache dotnet8-sdk postgresql16-client curl wget git bash
   ```

2. Restore .NET dependencies:
   ```bash
   dotnet restore
   ```

3. Build the solution:
   ```bash
   dotnet build
   ```

## Database Setup

The application uses PostgreSQL as its database. You need to set up PostgreSQL before running the application:

### Option 1: Using Docker (Recommended for development)

1. Build and run with docker-compose:
   ```bash
   docker-compose up -d
   ```

### Option 2: Local PostgreSQL installation

1. Run the PostgreSQL setup script:
   ```bash
   ./setup-postgres.sh
   ```

### Option 3: Manual PostgreSQL setup

1. Install PostgreSQL:
   ```bash
   apk update
   apk add --no-cache postgresql16 postgresql16-dev
   ```

2. Initialize and start PostgreSQL:
   ```bash
   adduser -D -H postgres
   mkdir -p /var/lib/postgresql/data
   chown -R postgres:postgres /var/lib/postgresql
   su - postgres -c "initdb -D /var/lib/postgresql/data"
   mkdir -p /run/postgresql
   chown -R postgres:postgres /run/postgresql
   echo "unix_socket_directories = '/run/postgresql'" >> /var/lib/postgresql/data/postgresql.conf
   su - postgres -c "pg_ctl -D /var/lib/postgresql/data -l /var/lib/postgresql/data/logfile start"
   ```

3. Create database and user:
   ```bash
   su - postgres -c "psql -c \"CREATE DATABASE happydaydb;\""
   su - postgres -c "psql -c \"ALTER USER postgres WITH PASSWORD 'postgres';\""
   su - postgres -c "psql -c \"GRANT ALL PRIVILEGES ON DATABASE happydaydb TO postgres;\""
   ```

## Database Migrations

This project includes a script to help manage Entity Framework Core migrations:

```bash
./migrate-database.sh [COMMAND]
```

See [README-MIGRATIONS.md](README-MIGRATIONS.md) for detailed usage instructions.

## Running the Application

After setting up the database, you can run the application with:

```bash
cd Presentation/HappyDay.Api
dotnet run
```

The API will be available at `http://localhost:5000` by default.

## Using Docker (Alternative)

If you prefer to run the application in Docker:

1. Build and run with docker-compose:
   ```bash
   docker-compose up -d
   ```

The API will be available at `http://localhost:8080`.