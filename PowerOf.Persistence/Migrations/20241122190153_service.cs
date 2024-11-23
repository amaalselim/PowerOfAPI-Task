using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerOf.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class service : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "services");

            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "services",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ServiceType",
                table: "services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                table: "services");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "services");

            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "services");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "services",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
