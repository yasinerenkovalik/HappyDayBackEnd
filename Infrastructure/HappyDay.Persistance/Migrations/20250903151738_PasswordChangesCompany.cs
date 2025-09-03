using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HappyDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class PasswordChangesCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Companies",
                newName: "PasswordHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Companies",
                newName: "Password");
        }
    }
}
