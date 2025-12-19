using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chibegir.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Sources_SourceId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sources",
                table: "Sources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Sources",
                newName: "Source");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SourceId",
                table: "Product",
                newName: "IX_Product_SourceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Source",
                table: "Source",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductSource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SourceId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSource_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSource_Source_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Source",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSource_ProductId",
                table: "ProductSource",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSource_SourceId",
                table: "ProductSource",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Source_SourceId",
                table: "Product",
                column: "SourceId",
                principalTable: "Source",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Source_SourceId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "ProductSource");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Source",
                table: "Source");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "Source",
                newName: "Sources");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameIndex(
                name: "IX_Product_SourceId",
                table: "Products",
                newName: "IX_Products_SourceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sources",
                table: "Sources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Sources_SourceId",
                table: "Products",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
