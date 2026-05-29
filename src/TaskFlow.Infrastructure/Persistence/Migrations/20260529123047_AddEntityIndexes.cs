using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskFlow.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_BoardColumnId_Order",
                table: "TaskItems",
                columns: new[] { "BoardColumnId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_BoardColumns_BoardId_Order",
                table: "BoardColumns",
                columns: new[] { "BoardId", "Order" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TaskItems_BoardColumnId_Order",
                table: "TaskItems");

            migrationBuilder.DropIndex(
                name: "IX_BoardColumns_BoardId_Order",
                table: "BoardColumns");
        }
    }
}
