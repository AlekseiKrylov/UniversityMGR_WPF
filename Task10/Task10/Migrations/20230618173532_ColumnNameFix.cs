using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task10.Migrations
{
    public partial class ColumnNameFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SurName",
                table: "Teachers",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "SurName",
                table: "Students",
                newName: "Surname");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Teachers",
                newName: "SurName");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Students",
                newName: "SurName");
        }
    }
}
