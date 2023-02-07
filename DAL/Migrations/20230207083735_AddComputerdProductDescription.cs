using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddComputerdProductDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "s_Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "[s_Name] + ' ' + STR([Price]) + 'zł'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "s_Description",
                table: "Products");
        }
    }
}
