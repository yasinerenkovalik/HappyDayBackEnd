#!/bin/bash

# Script to set up PostgreSQL for HappyDay application

echo "Setting up PostgreSQL for HappyDay..."

# Create postgres user if it doesn't exist
if ! id -u postgres >/dev/null 2>&1; then
    echo "Creating postgres user..."
    adduser -D -H postgres
fi

# Create data directory
mkdir -p /var/lib/postgresql/data
chown -R postgres:postgres /var/lib/postgresql

# Initialize database if not already initialized
if [ ! -f /var/lib/postgresql/data/PG_VERSION ]; then
    echo "Initializing PostgreSQL database..."
    su - postgres -c "initdb -D /var/lib/postgresql/data"
fi

# Create run directory
mkdir -p /run/postgresql
chown -R postgres:postgres /run/postgresql

# Update PostgreSQL configuration if needed
if ! grep -q "unix_socket_directories" /var/lib/postgresql/data/postgresql.conf; then
    echo "unix_socket_directories = '/run/postgresql'" >> /var/lib/postgresql/data/postgresql.conf
fi

# Start PostgreSQL
echo "Starting PostgreSQL..."
su - postgres -c "pg_ctl -D /var/lib/postgresql/data -l /var/lib/postgresql/data/logfile start"

# Wait for PostgreSQL to start
sleep 5

# Create database and user
echo "Creating database and user..."
su - postgres -c "psql -c \"CREATE DATABASE happydaydb;\"" || echo "Database already exists"
su - postgres -c "psql -c \"ALTER USER postgres WITH PASSWORD 'postgres';\""
su - postgres -c "psql -c \"GRANT ALL PRIVILEGES ON DATABASE happydaydb TO postgres;\""

echo "PostgreSQL setup complete!"
echo "You can now run the HappyDay application."