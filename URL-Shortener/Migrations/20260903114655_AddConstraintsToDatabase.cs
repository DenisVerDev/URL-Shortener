using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URL_Shortener.Migrations
{
    /// <inheritdoc />
    public partial class AddConstraintsToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ShortURLId",
                table: "URLs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OriginalURL",
                table: "URLs",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "URLs",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Login_Characters",
                table: "Users",
                sql: "[Login] COLLATE Latin1_General_100_BIN2 NOT LIKE '%[^A-Za-z0-9]%'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Login_Length",
                table: "Users",
                sql: "LEN([Login]) >= 6");

            migrationBuilder.CreateIndex(
                name: "IX_URLs_ShortURLId",
                table: "URLs",
                column: "ShortURLId",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_URL_OriginalURL_NotEmpty",
                table: "URLs",
                sql: "LEN(TRIM([OriginalURL])) > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_URL_ShortUrlId_NotEmpty",
                table: "URLs",
                sql: "LEN(TRIM([ShortURLId])) > 0");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Role_Name_NotEmpty",
                table: "Roles",
                sql: "LEN(TRIM([Name])) > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Login",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Login_Characters",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Login_Length",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_URLs_ShortURLId",
                table: "URLs");

            migrationBuilder.DropCheckConstraint(
                name: "CK_URL_OriginalURL_NotEmpty",
                table: "URLs");

            migrationBuilder.DropCheckConstraint(
                name: "CK_URL_ShortUrlId_NotEmpty",
                table: "URLs");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Name",
                table: "Roles");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Role_Name_NotEmpty",
                table: "Roles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "ShortURLId",
                table: "URLs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "OriginalURL",
                table: "URLs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "URLs",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
