using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Sales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialSalesRefactored : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sales");

            migrationBuilder.CreateTable(
                name: "station_category_configs",
                schema: "sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompanyCen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CategoryCen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Station = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_station_category_configs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tax_configurations",
                schema: "sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompanyCen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    GlobalTaxRate = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tax_configurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                schema: "sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompanyCen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerCen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    WaiterCen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DailyNumber = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AppliedTaxRate = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                schema: "sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TicketId = table.Column<int>(type: "integer", nullable: false),
                    Method = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_payments_tickets_TicketId",
                        column: x => x.TicketId,
                        principalSchema: "sales",
                        principalTable: "tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ticket_lines",
                schema: "sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TicketId = table.Column<int>(type: "integer", nullable: false),
                    CommandNumber = table.Column<int>(type: "integer", nullable: true),
                    ProductCen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProductName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Station = table.Column<string>(type: "text", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticket_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ticket_lines_tickets_TicketId",
                        column: x => x.TicketId,
                        principalSchema: "sales",
                        principalTable: "tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_payments_Cen",
                schema: "sales",
                table: "payments",
                column: "Cen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_TicketId",
                schema: "sales",
                table: "payments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_station_category_configs_Cen",
                schema: "sales",
                table: "station_category_configs",
                column: "Cen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tax_configurations_Cen",
                schema: "sales",
                table: "tax_configurations",
                column: "Cen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ticket_lines_Cen",
                schema: "sales",
                table: "ticket_lines",
                column: "Cen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ticket_lines_TicketId",
                schema: "sales",
                table: "ticket_lines",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_Cen",
                schema: "sales",
                table: "tickets",
                column: "Cen",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payments",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "station_category_configs",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "tax_configurations",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "ticket_lines",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "tickets",
                schema: "sales");
        }
    }
}
