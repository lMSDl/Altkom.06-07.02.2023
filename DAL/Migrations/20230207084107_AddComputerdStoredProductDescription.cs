using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddComputerdStoredProductDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "s_Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "[s_Name] + ' ' + STR([Price]) + 'zł'",
                stored: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComputedColumnSql: "[s_Name] + ' ' + STR([Price]) + 'zł'",
                oldStored: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "s_Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "[s_Name] + ' ' + STR([Price]) + 'zł'",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComputedColumnSql: "[s_Name] + ' ' + STR([Price]) + 'zł'");
        }
    }
}
