using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace cardapio_digital.Migrations
{
    /// <inheritdoc />
    public partial class addTabelasPaisEFilho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pais",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cpf = table.Column<string>(type: "text", nullable: false),
                    telefone = table.Column<string>(type: "text", nullable: false),
                    usuario_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pais", x => x.id);
                    table.ForeignKey(
                        name: "fk_pais_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "filho",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    data_nascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    telefone = table.Column<string>(type: "text", nullable: false),
                    pais_id = table.Column<int>(type: "integer", nullable: false),
                    escola_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_filho", x => x.id);
                    table.ForeignKey(
                        name: "fk_filho_escolas_escola_id",
                        column: x => x.escola_id,
                        principalTable: "escolas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_filho_pais_pais_id",
                        column: x => x.pais_id,
                        principalTable: "pais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_filho_escola_id",
                table: "filho",
                column: "escola_id");

            migrationBuilder.CreateIndex(
                name: "ix_filho_pais_id",
                table: "filho",
                column: "pais_id");

            migrationBuilder.CreateIndex(
                name: "ix_pais_usuario_id",
                table: "pais",
                column: "usuario_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "filho");

            migrationBuilder.DropTable(
                name: "pais");
        }
    }
}
