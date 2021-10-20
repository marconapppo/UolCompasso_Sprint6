using Microsoft.EntityFrameworkCore.Migrations;

namespace SisProdutos.Migrations
{
    public partial class Migrations3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "preco",
                table: "Produtos",
                newName: "Preco");

            migrationBuilder.RenameColumn(
                name: "palavraChave",
                table: "Produtos",
                newName: "PalavraChave");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "Produtos",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "descicao",
                table: "Produtos",
                newName: "Descicao");

            migrationBuilder.RenameColumn(
                name: "categoria",
                table: "Produtos",
                newName: "Categoria");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Preco",
                table: "Produtos",
                newName: "preco");

            migrationBuilder.RenameColumn(
                name: "PalavraChave",
                table: "Produtos",
                newName: "palavraChave");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Produtos",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "Descicao",
                table: "Produtos",
                newName: "descicao");

            migrationBuilder.RenameColumn(
                name: "Categoria",
                table: "Produtos",
                newName: "categoria");
        }
    }
}
