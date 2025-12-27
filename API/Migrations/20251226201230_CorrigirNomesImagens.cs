using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirNomesImagens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagemUrl",
                value: "/images/categorias/carteiras_fosforos.jpg");

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagemUrl",
                value: "/images/categorias/pacotes_açucar.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
