#!/bin/bash

# Dotnet Migration Script for HappyDay Project
# This script helps with creating and applying migrations

# Exit on any error
set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Print functions
print_info() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Check if dotnet is installed
if ! command -v dotnet &> /dev/null
then
    print_error "dotnet CLI could not be found. Please install .NET SDK."
    exit 1
fi

# Default values
PROJECT_PATH="../Infrastructure/HappyDay.Persistance/HappyDay.Persistance.csproj"
STARTUP_PROJECT_PATH="HappyDay.Api.csproj"
MIGRATIONS_DIR="../Infrastructure/HappyDay.Persistance/Migrations"
CONTEXT="HappyDayContext"
STARTUP_DIR="Presentation/HappyDay.Api"

# Show help
show_help() {
    echo "Usage: $0 [OPTIONS] COMMAND"
    echo ""
    echo "Dotnet Migration Script for HappyDay Project"
    echo ""
    echo "Commands:"
    echo "  add <MigrationName>     Add a new migration"
    echo "  update                  Update database to the latest migration"
    echo "  list                    List all migrations"
    echo "  remove                  Remove the last migration"
    echo "  script                  Generate SQL script for migrations"
    echo ""
    echo "Options:"
    echo "  -h, --help              Show this help message"
    echo ""
    echo "Examples:"
    echo "  $0 add InitialCreate"
    echo "  $0 update"
    echo "  $0 list"
}

# Add a new migration
add_migration() {
    if [ -z "$1" ]; then
        print_error "Migration name is required. Usage: $0 add <MigrationName>"
        exit 1
    fi
    
    MIGRATION_NAME=$1
    
    print_info "Adding migration: $MIGRATION_NAME"
    
    # Change to the startup project directory
    cd "$STARTUP_DIR"
    
    # Add migration
    ~/.dotnet/tools/dotnet-ef migrations add "$MIGRATION_NAME" --project "../$PROJECT_PATH" --startup-project "." --context "$CONTEXT" --output-dir "../$MIGRATIONS_DIR"
    
    # Return to original directory
    cd - > /dev/null
    
    print_info "Migration '$MIGRATION_NAME' added successfully!"
}

# Update database
update_database() {
    print_info "Updating database to the latest migration"
    
    # Change to the startup project directory
    cd "$STARTUP_DIR"
    
    # Apply migrations
    ~/.dotnet/tools/dotnet-ef database update --project "../$PROJECT_PATH" --startup-project "." --context "$CONTEXT"
    
    # Return to original directory
    cd - > /dev/null
    
    print_info "Database updated successfully!"
}

# List migrations
list_migrations() {
    print_info "Listing all migrations"
    
    # Change to the startup project directory
    cd "$STARTUP_DIR"
    
    # List migrations
    ~/.dotnet/tools/dotnet-ef migrations list --project "../$PROJECT_PATH" --startup-project "." --context "$CONTEXT"
    
    # Return to original directory
    cd - > /dev/null
}

# Remove last migration
remove_migration() {
    print_warning "Removing the last migration. This action cannot be undone."
    
    # Confirm with user
    read -p "Are you sure you want to remove the last migration? (y/N): " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Yy]$ ]]
    then
        print_info "Operation cancelled."
        exit 0
    fi
    
    # Change to the startup project directory
    cd "$STARTUP_DIR"
    
    # Remove migration
    ~/.dotnet/tools/dotnet-ef migrations remove --project "../$PROJECT_PATH" --startup-project "." --context "$CONTEXT"
    
    # Return to original directory
    cd - > /dev/null
    
    print_info "Last migration removed successfully!"
}

# Generate SQL script
generate_script() {
    print_info "Generating SQL script for migrations"
    
    # Change to the startup project directory
    cd "$STARTUP_DIR"
    
    # Generate script
    ~/.dotnet/tools/dotnet-ef migrations script --project "../$PROJECT_PATH" --startup-project "." --context "$CONTEXT" --output "../migration-script.sql"
    
    # Return to original directory
    cd - > /dev/null
    
    print_info "SQL script generated at migration-script.sql"
}

# Parse command line arguments
case "$1" in
    add)
        add_migration "$2"
        ;;
    update)
        update_database
        ;;
    list)
        list_migrations
        ;;
    remove)
        remove_migration
        ;;
    script)
        generate_script
        ;;
    -h|--help)
        show_help
        ;;
    *)
        if [ -z "$1" ]; then
            print_error "No command specified."
        else
            print_error "Unknown command: $1"
        fi
        show_help
        exit 1
        ;;
esac