using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chibegir.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Product_SourceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Source_SourceId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_SourceId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "Product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_SourceId",
                table: "Product",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Source_SourceId",
                table: "Product",
                column: "SourceId",
                principalTable: "Source",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
