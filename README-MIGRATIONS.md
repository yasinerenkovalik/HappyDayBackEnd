# Database Migration Script

This script helps with managing Entity Framework Core migrations for the HappyDay project.

## Usage

```bash
./migrate-database.sh [OPTIONS] COMMAND
```

## Commands

- `add <MigrationName>` - Add a new migration
- `update` - Update database to the latest migration
- `list` - List all migrations
- `remove` - Remove the last migration
- `script` - Generate SQL script for migrations

## Examples

```bash
# Add a new migration
./migrate-database.sh add AddUserTable

# Update database to the latest migration
./migrate-database.sh update

# List all migrations
./migrate-database.sh list

# Remove the last migration
./migrate-database.sh remove

# Generate SQL script for migrations
./migrate-database.sh script
```

## Prerequisites

- .NET SDK installed
- EF Core tools installed globally (`dotnet tool install --global dotnet-ef`)
- The script must be run from the project root directory