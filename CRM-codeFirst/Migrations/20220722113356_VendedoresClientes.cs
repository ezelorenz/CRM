using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM_codeFirst.Migrations
{
    public partial class VendedoresClientes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendedoresClientes",
                columns: table => new
                {
                    VendedorId = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    // Orden = table.Column<int>(type: "int", nullable: false),
                    FechaeAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendedoresClientes", x => new { x.VendedorId, x.ClienteId });
                    table.ForeignKey(
                        name: "FK_VendedoresClientes_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendedoresClientes_Vendedores_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Vendedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendedoresClientes_ClienteId",
                table: "VendedoresClientes",
                column: "ClienteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendedoresClientes");
        }
    }
}
