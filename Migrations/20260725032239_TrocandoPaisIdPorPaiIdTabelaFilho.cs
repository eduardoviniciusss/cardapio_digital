using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cardapio_digital.Migrations
{
    /// <inheritdoc />
    public partial class TrocandoPaisIdPorPaiIdTabelaFilho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_filhos_pais_pais_id",
                table: "filhos");

            migrationBuilder.RenameColumn(
                name: "pais_id",
                table: "filhos",
                newName: "pai_id");

            migrationBuilder.RenameIndex(
                name: "ix_filhos_pais_id",
                table: "filhos",
                newName: "ix_filhos_pai_id");

            migrationBuilder.AddForeignKey(
                name: "fk_filhos_pais_pai_id",
                table: "filhos",
                column: "pai_id",
                principalTable: "pais",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_filhos_pais_pai_id",
                table: "filhos");

            migrationBuilder.RenameColumn(
                name: "pai_id",
                table: "filhos",
                newName: "pais_id");

            migrationBuilder.RenameIndex(
                name: "ix_filhos_pai_id",
                table: "filhos",
                newName: "ix_filhos_pais_id");

            migrationBuilder.AddForeignKey(
                name: "fk_filhos_pais_pais_id",
                table: "filhos",
                column: "pais_id",
                principalTable: "pais",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
