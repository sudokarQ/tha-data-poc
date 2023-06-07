using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCrash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Crashes",
                table: "Crashes");

            migrationBuilder.DropColumn(
                name: "CrashId",
                table: "Crashes");

            migrationBuilder.DropColumn(
                name: "Weekend",
                table: "Crashes");

            migrationBuilder.AlterColumn<string>(
                name: "Longitude",
                table: "Crashes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Latitude",
                table: "Crashes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Hour",
                table: "Crashes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Crashes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Crashes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsWeekend",
                table: "Crashes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Crashes",
                table: "Crashes",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Crashes",
                table: "Crashes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Crashes");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Crashes");

            migrationBuilder.DropColumn(
                name: "IsWeekend",
                table: "Crashes");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Crashes",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Crashes",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Hour",
                table: "Crashes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CrashId",
                table: "Crashes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Weekend",
                table: "Crashes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Crashes",
                table: "Crashes",
                column: "CrashId");
        }
    }
}
