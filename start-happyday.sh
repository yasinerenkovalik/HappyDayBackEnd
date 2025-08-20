#!/bin/bash

# HappyDay Startup Script

echo "Starting HappyDay application..."

# Start PostgreSQL if not already running
if ! pg_isready > /dev/null 2>&1; then
    echo "Starting PostgreSQL..."
    mkdir -p /run/postgresql
    chown -R postgres:postgres /run/postgresql
    su - postgres -c "pg_ctl -D /var/lib/postgresql/data -l /var/lib/postgresql/data/logfile start"
    
    # Wait for PostgreSQL to start
    sleep 5
    
    # Check if PostgreSQL is ready
    if ! pg_isready > /dev/null 2>&1; then
        echo "Failed to start PostgreSQL. Exiting."
        exit 1
    fi
    echo "PostgreSQL started successfully."
else
    echo "PostgreSQL is already running."
fi

# Start the HappyDay API
echo "Starting HappyDay API..."
cd /workspace/HappyDayBackEnd/Presentation/HappyDay.Api
dotnet run