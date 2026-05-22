using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialInventoryRefactored : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "inventory");

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompanyCen = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompanyCen = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ContactInfo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "units",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompanyCen = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "warehouses",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompanyCen = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompanyCen = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    UnitId = table.Column<int>(type: "integer", nullable: false),
                    SupplierId = table.Column<int>(type: "integer", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    MinStockAlert = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_products_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "inventory",
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_products_suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "inventory",
                        principalTable: "suppliers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_products_units_UnitId",
                        column: x => x.UnitId,
                        principalSchema: "inventory",
                        principalTable: "units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_documents",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompanyCen = table.Column<string>(type: "text", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    DocumentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inventory_documents_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "inventory",
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "product_stocks",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompanyCen = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    CurrentQuantity = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_stocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_product_stocks_products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "inventory",
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_stocks_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "inventory",
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "inventory_document_lines",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DocumentId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_document_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inventory_document_lines_inventory_documents_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "inventory",
                        principalTable: "inventory_documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventory_document_lines_products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "inventory",
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "kardex_movements",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompanyCen = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    DocumentId = table.Column<int>(type: "integer", nullable: true),
                    MovementType = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    MovementDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kardex_movements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_kardex_movements_inventory_documents_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "inventory",
                        principalTable: "inventory_documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_kardex_movements_products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "inventory",
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_kardex_movements_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "inventory",
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_categories_Cen",
                schema: "inventory",
                table: "categories",
                column: "Cen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_inventory_document_lines_Cen",
                schema: "inventory",
                table: "inventory_document_lines",
                column: "Cen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_inventory_document_lines_DocumentId",
                schema: "inventory",
                table: "inventory_document_lines",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_document_lines_ProductId",
                schema: "inventory",
                table: "inventory_document_lines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_documents_Cen",
                schema: "inventory",
                table: "inventory_documents",
                column: "Cen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_inventory_documents_WarehouseId",
                schema: "inventory",
                table: "inventory_documents",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_kardex_movements_Cen",
                schema: "inventory",
                table: "kardex_movements",
                column: "Cen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_kardex_movements_DocumentId",
                schema: "inventory",
                table: "kardex_movements",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_kardex_movements_ProductId",
                schema: "inventory",
                table: "kardex_movements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_kardex_movements_WarehouseId",
                schema: "inventory",
                table: "kardex_movements",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_product_stocks_Cen",
                schema: "inventory",
                table: "product_stocks",
                column: "Cen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_stocks_ProductId_WarehouseId",
                schema: "inventory",
                table: "product_stocks",
                columns: new[] { "ProductId", "WarehouseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_stocks_WarehouseId",
                schema: "inventory",
                table: "product_stocks",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_products_CategoryId",
                schema: "inventory",
                table: "products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_products_Cen",
                schema: "inventory",
                table: "products",
                column: "Cen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_SupplierId",
                schema: "inventory",
                table: "products",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_products_UnitId",
                schema: "inventory",
                table: "products",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_suppliers_Cen",
                schema: "inventory",
                table: "suppliers",
                column: "Cen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_units_Cen",
                schema: "inventory",
                table: "units",
                column: "Cen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_warehouses_Cen",
                schema: "inventory",
                table: "warehouses",
                column: "Cen",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventory_document_lines",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "kardex_movements",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "product_stocks",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "inventory_documents",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "products",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "warehouses",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "suppliers",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "units",
                schema: "inventory");
        }
    }
}
