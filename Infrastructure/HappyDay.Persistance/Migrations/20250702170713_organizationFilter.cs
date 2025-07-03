using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HappyDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class organizationFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Organizations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "Organizations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Organizations");
        }
    }
}
