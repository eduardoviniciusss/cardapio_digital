using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cardapio_digital.Migrations
{
    /// <inheritdoc />
    public partial class RenomendoPortuguesPraEnglish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cardapio_produtos_cardapios_cardapio_id",
                table: "cardapio_produtos");

            migrationBuilder.DropForeignKey(
                name: "fk_cardapio_produtos_produtos_produto_id",
                table: "cardapio_produtos");

            migrationBuilder.DropForeignKey(
                name: "fk_cardapios_escolas_escola_id",
                table: "cardapios");

            migrationBuilder.DropForeignKey(
                name: "fk_categorias_escolas_escola_id",
                table: "categorias");

            migrationBuilder.DropForeignKey(
                name: "fk_escolas_usuarios_usuario_id",
                table: "escolas");

            migrationBuilder.DropForeignKey(
                name: "fk_filhos_escolas_escola_id",
                table: "filhos");

            migrationBuilder.DropForeignKey(
                name: "fk_filhos_pais_pai_id",
                table: "filhos");

            migrationBuilder.DropForeignKey(
                name: "fk_pais_usuarios_usuario_id",
                table: "pais");

            migrationBuilder.DropForeignKey(
                name: "fk_produtos_categorias_categoria_id",
                table: "produtos");

            migrationBuilder.DropForeignKey(
                name: "fk_produtos_escolas_escola_id",
                table: "produtos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_usuarios",
                table: "usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "pk_produtos",
                table: "produtos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_pais",
                table: "pais");

            migrationBuilder.DropPrimaryKey(
                name: "pk_filhos",
                table: "filhos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_escolas",
                table: "escolas");

            migrationBuilder.DropPrimaryKey(
                name: "pk_categorias",
                table: "categorias");

            migrationBuilder.DropPrimaryKey(
                name: "pk_cardapios",
                table: "cardapios");

            migrationBuilder.DropPrimaryKey(
                name: "pk_cardapio_produtos",
                table: "cardapio_produtos");

            migrationBuilder.RenameTable(
                name: "usuarios",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "produtos",
                newName: "products");

            migrationBuilder.RenameTable(
                name: "pais",
                newName: "parents");

            migrationBuilder.RenameTable(
                name: "filhos",
                newName: "children");

            migrationBuilder.RenameTable(
                name: "escolas",
                newName: "schools");

            migrationBuilder.RenameTable(
                name: "categorias",
                newName: "categories");

            migrationBuilder.RenameTable(
                name: "cardapios",
                newName: "menus");

            migrationBuilder.RenameTable(
                name: "cardapio_produtos",
                newName: "menu_products");

            migrationBuilder.RenameColumn(
                name: "senha_hash",
                table: "user",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "perfil",
                table: "user",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "user",
                newName: "name");

            migrationBuilder.RenameIndex(
                name: "ix_usuarios_email",
                table: "user",
                newName: "ix_user_email");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "products",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "preco",
                table: "products",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "escola_id",
                table: "products",
                newName: "school_id");

            migrationBuilder.RenameColumn(
                name: "categoria_id",
                table: "products",
                newName: "category_id");

            migrationBuilder.RenameIndex(
                name: "ix_produtos_escola_id",
                table: "products",
                newName: "ix_products_school_id");

            migrationBuilder.RenameIndex(
                name: "ix_produtos_categoria_id",
                table: "products",
                newName: "ix_products_category_id");

            migrationBuilder.RenameColumn(
                name: "usuario_id",
                table: "parents",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "telefone",
                table: "parents",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "parents",
                newName: "name");

            migrationBuilder.RenameIndex(
                name: "ix_pais_usuario_id",
                table: "parents",
                newName: "ix_parents_user_id");

            migrationBuilder.RenameColumn(
                name: "telefone",
                table: "children",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "pai_id",
                table: "children",
                newName: "parent_id");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "children",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "data_nascimento",
                table: "children",
                newName: "birth_date");

            migrationBuilder.RenameColumn(
                name: "escola_id",
                table: "children",
                newName: "school_id");

            migrationBuilder.RenameIndex(
                name: "ix_filhos_pai_id",
                table: "children",
                newName: "ix_children_parent_id");

            migrationBuilder.RenameIndex(
                name: "ix_filhos_escola_id",
                table: "children",
                newName: "ix_children_school_id");

            migrationBuilder.RenameColumn(
                name: "usuario_id",
                table: "schools",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "turnos",
                table: "schools",
                newName: "shifts");

            migrationBuilder.RenameColumn(
                name: "telefone",
                table: "schools",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "schools",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "endereco",
                table: "schools",
                newName: "address");

            migrationBuilder.RenameIndex(
                name: "ix_escolas_usuario_id",
                table: "schools",
                newName: "ix_schools_user_id");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "categories",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "escola_id",
                table: "categories",
                newName: "school_id");

            migrationBuilder.RenameIndex(
                name: "ix_categorias_escola_id",
                table: "categories",
                newName: "ix_categories_school_id");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "menus",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "escola_id",
                table: "menus",
                newName: "school_id");

            migrationBuilder.RenameIndex(
                name: "ix_cardapios_escola_id",
                table: "menus",
                newName: "ix_menus_school_id");

            migrationBuilder.RenameColumn(
                name: "produto_id",
                table: "menu_products",
                newName: "product_id");

            migrationBuilder.RenameColumn(
                name: "cardapio_id",
                table: "menu_products",
                newName: "menu_id");

            migrationBuilder.RenameIndex(
                name: "ix_cardapio_produtos_produto_id",
                table: "menu_products",
                newName: "ix_menu_products_product_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user",
                table: "user",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_products",
                table: "products",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_parents",
                table: "parents",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_children",
                table: "children",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_schools",
                table: "schools",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_categories",
                table: "categories",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_menus",
                table: "menus",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_menu_products",
                table: "menu_products",
                columns: new[] { "menu_id", "product_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_categories_schools_school_id",
                table: "categories",
                column: "school_id",
                principalTable: "schools",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_children_parents_parent_id",
                table: "children",
                column: "parent_id",
                principalTable: "parents",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_children_schools_school_id",
                table: "children",
                column: "school_id",
                principalTable: "schools",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_menu_products_menus_menu_id",
                table: "menu_products",
                column: "menu_id",
                principalTable: "menus",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_menu_products_products_product_id",
                table: "menu_products",
                column: "product_id",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_menus_schools_school_id",
                table: "menus",
                column: "school_id",
                principalTable: "schools",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_parents_user_user_id",
                table: "parents",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_products_categories_category_id",
                table: "products",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_products_schools_school_id",
                table: "products",
                column: "school_id",
                principalTable: "schools",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_schools_user_user_id",
                table: "schools",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_categories_schools_school_id",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "fk_children_parents_parent_id",
                table: "children");

            migrationBuilder.DropForeignKey(
                name: "fk_children_schools_school_id",
                table: "children");

            migrationBuilder.DropForeignKey(
                name: "fk_menu_products_menus_menu_id",
                table: "menu_products");

            migrationBuilder.DropForeignKey(
                name: "fk_menu_products_products_product_id",
                table: "menu_products");

            migrationBuilder.DropForeignKey(
                name: "fk_menus_schools_school_id",
                table: "menus");

            migrationBuilder.DropForeignKey(
                name: "fk_parents_user_user_id",
                table: "parents");

            migrationBuilder.DropForeignKey(
                name: "fk_products_categories_category_id",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "fk_products_schools_school_id",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "fk_schools_user_user_id",
                table: "schools");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "pk_schools",
                table: "schools");

            migrationBuilder.DropPrimaryKey(
                name: "pk_products",
                table: "products");

            migrationBuilder.DropPrimaryKey(
                name: "pk_parents",
                table: "parents");

            migrationBuilder.DropPrimaryKey(
                name: "pk_menus",
                table: "menus");

            migrationBuilder.DropPrimaryKey(
                name: "pk_menu_products",
                table: "menu_products");

            migrationBuilder.DropPrimaryKey(
                name: "pk_children",
                table: "children");

            migrationBuilder.DropPrimaryKey(
                name: "pk_categories",
                table: "categories");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "usuarios");

            migrationBuilder.RenameTable(
                name: "schools",
                newName: "escolas");

            migrationBuilder.RenameTable(
                name: "products",
                newName: "produtos");

            migrationBuilder.RenameTable(
                name: "parents",
                newName: "pais");

            migrationBuilder.RenameTable(
                name: "menus",
                newName: "cardapios");

            migrationBuilder.RenameTable(
                name: "menu_products",
                newName: "cardapio_produtos");

            migrationBuilder.RenameTable(
                name: "children",
                newName: "filhos");

            migrationBuilder.RenameTable(
                name: "categories",
                newName: "categorias");

            migrationBuilder.RenameColumn(
                name: "role",
                table: "usuarios",
                newName: "perfil");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "usuarios",
                newName: "senha_hash");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "usuarios",
                newName: "nome");

            migrationBuilder.RenameIndex(
                name: "ix_user_email",
                table: "usuarios",
                newName: "ix_usuarios_email");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "escolas",
                newName: "usuario_id");

            migrationBuilder.RenameColumn(
                name: "shifts",
                table: "escolas",
                newName: "turnos");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "escolas",
                newName: "telefone");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "escolas",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "escolas",
                newName: "endereco");

            migrationBuilder.RenameIndex(
                name: "ix_schools_user_id",
                table: "escolas",
                newName: "ix_escolas_usuario_id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "produtos",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "school_id",
                table: "produtos",
                newName: "escola_id");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "produtos",
                newName: "preco");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "produtos",
                newName: "categoria_id");

            migrationBuilder.RenameIndex(
                name: "ix_products_school_id",
                table: "produtos",
                newName: "ix_produtos_escola_id");

            migrationBuilder.RenameIndex(
                name: "ix_products_category_id",
                table: "produtos",
                newName: "ix_produtos_categoria_id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "pais",
                newName: "usuario_id");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "pais",
                newName: "telefone");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "pais",
                newName: "nome");

            migrationBuilder.RenameIndex(
                name: "ix_parents_user_id",
                table: "pais",
                newName: "ix_pais_usuario_id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "cardapios",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "school_id",
                table: "cardapios",
                newName: "escola_id");

            migrationBuilder.RenameIndex(
                name: "ix_menus_school_id",
                table: "cardapios",
                newName: "ix_cardapios_escola_id");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "cardapio_produtos",
                newName: "produto_id");

            migrationBuilder.RenameColumn(
                name: "menu_id",
                table: "cardapio_produtos",
                newName: "cardapio_id");

            migrationBuilder.RenameIndex(
                name: "ix_menu_products_product_id",
                table: "cardapio_produtos",
                newName: "ix_cardapio_produtos_produto_id");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "filhos",
                newName: "telefone");

            migrationBuilder.RenameColumn(
                name: "parent_id",
                table: "filhos",
                newName: "pai_id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "filhos",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "birth_date",
                table: "filhos",
                newName: "data_nascimento");

            migrationBuilder.RenameColumn(
                name: "school_id",
                table: "filhos",
                newName: "escola_id");

            migrationBuilder.RenameIndex(
                name: "ix_children_school_id",
                table: "filhos",
                newName: "ix_filhos_escola_id");

            migrationBuilder.RenameIndex(
                name: "ix_children_parent_id",
                table: "filhos",
                newName: "ix_filhos_pai_id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "categorias",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "school_id",
                table: "categorias",
                newName: "escola_id");

            migrationBuilder.RenameIndex(
                name: "ix_categories_school_id",
                table: "categorias",
                newName: "ix_categorias_escola_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_usuarios",
                table: "usuarios",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_escolas",
                table: "escolas",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_produtos",
                table: "produtos",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_pais",
                table: "pais",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_cardapios",
                table: "cardapios",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_cardapio_produtos",
                table: "cardapio_produtos",
                columns: new[] { "cardapio_id", "produto_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_filhos",
                table: "filhos",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_categorias",
                table: "categorias",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_cardapio_produtos_cardapios_cardapio_id",
                table: "cardapio_produtos",
                column: "cardapio_id",
                principalTable: "cardapios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_cardapio_produtos_produtos_produto_id",
                table: "cardapio_produtos",
                column: "produto_id",
                principalTable: "produtos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_cardapios_escolas_escola_id",
                table: "cardapios",
                column: "escola_id",
                principalTable: "escolas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_categorias_escolas_escola_id",
                table: "categorias",
                column: "escola_id",
                principalTable: "escolas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_escolas_usuarios_usuario_id",
                table: "escolas",
                column: "usuario_id",
                principalTable: "usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_filhos_escolas_escola_id",
                table: "filhos",
                column: "escola_id",
                principalTable: "escolas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_filhos_pais_pai_id",
                table: "filhos",
                column: "pai_id",
                principalTable: "pais",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_pais_usuarios_usuario_id",
                table: "pais",
                column: "usuario_id",
                principalTable: "usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_produtos_categorias_categoria_id",
                table: "produtos",
                column: "categoria_id",
                principalTable: "categorias",
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
    }
}
