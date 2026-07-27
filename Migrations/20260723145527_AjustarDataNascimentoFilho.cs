using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cardapio_digital.Migrations
{
    /// <inheritdoc />
    public partial class AjustarDataNascimentoFilho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_filho_escolas_escola_id",
                table: "filho");

            migrationBuilder.DropForeignKey(
                name: "fk_filho_pais_pais_id",
                table: "filho");

            migrationBuilder.DropPrimaryKey(
                name: "pk_filho",
                table: "filho");

            migrationBuilder.RenameTable(
                name: "filho",
                newName: "filhos");

            migrationBuilder.RenameIndex(
                name: "ix_filho_pais_id",
                table: "filhos",
                newName: "ix_filhos_pais_id");

            migrationBuilder.RenameIndex(
                name: "ix_filho_escola_id",
                table: "filhos",
                newName: "ix_filhos_escola_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "data_nascimento",
                table: "filhos",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "nome",
                table: "filhos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "pk_filhos",
                table: "filhos",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_filhos_escolas_escola_id",
                table: "filhos",
                column: "escola_id",
                principalTable: "escolas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_filhos_pais_pais_id",
                table: "filhos",
                column: "pais_id",
                principalTable: "pais",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_filhos_escolas_escola_id",
                table: "filhos");

            migrationBuilder.DropForeignKey(
                name: "fk_filhos_pais_pais_id",
                table: "filhos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_filhos",
                table: "filhos");

            migrationBuilder.DropColumn(
                name: "nome",
                table: "filhos");

            migrationBuilder.RenameTable(
                name: "filhos",
                newName: "filho");

            migrationBuilder.RenameIndex(
                name: "ix_filhos_pais_id",
                table: "filho",
                newName: "ix_filho_pais_id");

            migrationBuilder.RenameIndex(
                name: "ix_filhos_escola_id",
                table: "filho",
                newName: "ix_filho_escola_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "data_nascimento",
                table: "filho",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddPrimaryKey(
                name: "pk_filho",
                table: "filho",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_filho_escolas_escola_id",
                table: "filho",
                column: "escola_id",
                principalTable: "escolas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_filho_pais_pais_id",
                table: "filho",
                column: "pais_id",
                principalTable: "pais",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
