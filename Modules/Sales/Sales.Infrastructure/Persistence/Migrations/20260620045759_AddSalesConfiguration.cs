using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Sales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesConfigurations",
                schema: "sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompanyCen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DefaultWarehouseCen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesConfigurations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesConfigurations_Cen",
                schema: "sales",
                table: "SalesConfigurations",
                column: "Cen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesConfigurations_CompanyCen",
                schema: "sales",
                table: "SalesConfigurations",
                column: "CompanyCen",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesConfigurations",
                schema: "sales");
        }
    }
}
