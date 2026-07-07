using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cardapio_digital.Migrations
{
    /// <inheritdoc />
    public partial class RelacionamentoUsuarioEscola : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "usuario_id",
                table: "escolas",
                type: "integer",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "ix_escolas_usuario_id",
                table: "escolas",
                column: "usuario_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_escolas_usuarios_usuario_id",
                table: "escolas",
                column: "usuario_id",
                principalTable: "usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_escolas_usuarios_usuario_id",
                table: "escolas");

            migrationBuilder.DropIndex(
                name: "ix_escolas_usuario_id",
                table: "escolas");

            migrationBuilder.DropColumn(
                name: "usuario_id",
                table: "escolas");
        }
    }
}
