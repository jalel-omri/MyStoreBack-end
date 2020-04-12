using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyStore.Migrations
{
    public partial class CreationInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "produits",
                columns: table => new
                {
                    id = table.Column<string>(fixedLength: true, maxLength: 10, nullable: false),
                    improd = table.Column<byte[]>(type: "image", nullable: false),
                    namprod = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    descprod = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    priprod = table.Column<decimal>(type: "numeric(18, 0)", nullable: false),
                    discprod = table.Column<decimal>(type: "numeric(18, 0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produits", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "produits");
        }
    }
}
