using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cardapio_digital.Migrations
{
    /// <inheritdoc />
    public partial class AddSchoolCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "schools",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");

                migrationBuilder.Sql(
                "UPDATE schools SET code = upper(substr(md5(random()::text || id::text), 1, 8)) WHERE code = '';");

            migrationBuilder.CreateIndex(
                name: "ix_schools_code",
                table: "schools",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_schools_code",
                table: "schools");

            migrationBuilder.DropColumn(
                name: "code",
                table: "schools");
        }
    }
}
