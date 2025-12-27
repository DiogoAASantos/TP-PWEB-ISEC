using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirImagensCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagemUrl",
                table: "Categoria",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagemUrl",
                value: "/images/categorias/moedas.jpg");

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagemUrl",
                value: "/images/categorias/selos.jpg");

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagemUrl",
                value: "/images/categorias/carteiras_fosforo.jpg");

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagemUrl",
                value: "/images/categorias/pacotes_acucar.jpg");

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImagemUrl",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemUrl",
                table: "Categoria");
        }
    }
}
