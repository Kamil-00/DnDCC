using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ItemUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Item");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Item",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Item",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
