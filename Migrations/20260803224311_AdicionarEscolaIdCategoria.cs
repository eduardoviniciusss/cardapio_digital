using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cardapio_digital.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarEscolaIdCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "escola_id",
                table: "produtos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "escola_id",
                table: "categorias",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_produtos_escola_id",
                table: "produtos",
                column: "escola_id");

            migrationBuilder.CreateIndex(
                name: "ix_categorias_escola_id",
                table: "categorias",
                column: "escola_id");

            migrationBuilder.AddForeignKey(
                name: "fk_categorias_escolas_escola_id",
                table: "categorias",
                column: "escola_id",
                principalTable: "escolas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_produtos_escolas_escola_id",
                table: "produtos",
                column: "escola_id",
                principalTable: "escolas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_categorias_escolas_escola_id",
                table: "categorias");

            migrationBuilder.DropForeignKey(
                name: "fk_produtos_escolas_escola_id",
                table: "produtos");

            migrationBuilder.DropIndex(
                name: "ix_produtos_escola_id",
                table: "produtos");

            migrationBuilder.DropIndex(
                name: "ix_categorias_escola_id",
                table: "categorias");

            migrationBuilder.DropColumn(
                name: "escola_id",
                table: "produtos");

            migrationBuilder.DropColumn(
                name: "escola_id",
                table: "categorias");
        }
    }
}
