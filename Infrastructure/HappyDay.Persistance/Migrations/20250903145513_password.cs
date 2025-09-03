using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HappyDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class password : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // CompanyInvitations: güvenli kolon silme (varsa siler, yoksa hata vermez)
            migrationBuilder.Sql(@"ALTER TABLE ""CompanyInvitations"" DROP COLUMN IF EXISTS ""Password"";");
            migrationBuilder.Sql(@"ALTER TABLE ""CompanyInvitations"" DROP COLUMN IF EXISTS ""PhoneNumber"";");

            // Users: Password -> PasswordHash (koşullu rename)
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM information_schema.columns 
        WHERE table_name = 'Users' AND column_name = 'Password'
    ) AND NOT EXISTS (
        SELECT 1 FROM information_schema.columns 
        WHERE table_name = 'Users' AND column_name = 'PasswordHash'
    ) THEN
        ALTER TABLE ""Users"" RENAME COLUMN ""Password"" TO ""PasswordHash"";
    END IF;
END $$;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Users: PasswordHash -> Password (koşullu geri alma)
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM information_schema.columns 
        WHERE table_name = 'Users' AND column_name = 'PasswordHash'
    ) AND NOT EXISTS (
        SELECT 1 FROM information_schema.columns 
        WHERE table_name = 'Users' AND column_name = 'Password'
    ) THEN
        ALTER TABLE ""Users"" RENAME COLUMN ""PasswordHash"" TO ""Password"";
    END IF;
END $$;
");

            // CompanyInvitations: kolonları geri ekle (gerekliyse)
            migrationBuilder.Sql(@"ALTER TABLE ""CompanyInvitations"" ADD COLUMN IF NOT EXISTS ""Password"" text NOT NULL DEFAULT '';");
            migrationBuilder.Sql(@"ALTER TABLE ""CompanyInvitations"" ADD COLUMN IF NOT EXISTS ""PhoneNumber"" text NOT NULL DEFAULT '';");
        }
    }
}
