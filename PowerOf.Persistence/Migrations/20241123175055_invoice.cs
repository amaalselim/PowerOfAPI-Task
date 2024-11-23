using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerOf.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class invoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ordersItem_services_ServiceId",
                table: "ordersItem");

            migrationBuilder.DropIndex(
                name: "IX_ordersItem_ServiceId",
                table: "ordersItem");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "ordersItem");

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_invoices_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_invoices_OrderId",
                table: "invoices",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "ordersItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ordersItem_ServiceId",
                table: "ordersItem",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ordersItem_services_ServiceId",
                table: "ordersItem",
                column: "ServiceId",
                principalTable: "services",
                principalColumn: "Id");
        }
    }
}
