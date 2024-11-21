using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beat_backend_game.Migrations
{
    /// <inheritdoc />
    public partial class ModelBuilder9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdTipo",
                table: "VerdadeiroFalsos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdTipo",
                table: "OrdemPalavras",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdTipo",
                table: "EscolhaMultiplas",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "VerdadeiroFalsos",
                newName: "IdTipo");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "OrdemPalavras",
                newName: "IdTipo");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "EscolhaMultiplas",
                newName: "IdTipo");
        }
    }
}
