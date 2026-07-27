using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cardapio_digital.Migrations
{
    /// <inheritdoc />
    public partial class AddNomeEmPais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "nome",
                table: "pais",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nome",
                table: "pais");
        }
    }
}
