using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HappyDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class organizationUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Categories_CategoryId",
                table: "Organizations");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Organizations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancelPolicy",
                table: "Organizations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "Organizations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsOutdoor",
                table: "Organizations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Organizations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReservationNote",
                table: "Organizations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string[]>(
                name: "Services",
                table: "Organizations",
                type: "text[]",
                nullable: false,
                defaultValue: new string[] {} // ✅ boş dizi
            );

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Organizations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Categories_CategoryId",
                table: "Organizations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Categories_CategoryId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CancelPolicy",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "IsOutdoor",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "ReservationNote",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Services",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Organizations");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Organizations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Categories_CategoryId",
                table: "Organizations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
