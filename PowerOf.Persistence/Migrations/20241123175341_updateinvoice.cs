using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerOf.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateinvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "invoices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
