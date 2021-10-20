using Microsoft.EntityFrameworkCore.Migrations;

namespace SisProdutos.Migrations
{
    public partial class Migrations5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descicao",
                table: "Produtos",
                newName: "Descricao");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "Produtos",
                newName: "Descicao");
        }
    }
}
