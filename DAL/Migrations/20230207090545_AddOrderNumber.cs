using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddOrderNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sequences");

            migrationBuilder.CreateSequence<int>(
                name: "OrderNumber",
                schema: "sequences",
                startValue: 100L,
                incrementBy: 333,
                minValue: 0L,
                maxValue: 999L,
                cyclic: true);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR sequences.OrderNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "OrderNumber",
                schema: "sequences");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Orders");
        }
    }
}
