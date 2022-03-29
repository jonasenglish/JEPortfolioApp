using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JEPortfolioApp.Data.Migrations
{
    public partial class projectsmonthyear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Project",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Project",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Project");
        }
    }
}
