using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerManagement.Migrations
{
    /// <inheritdoc />
    public partial class removecustomerid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "FeeRecords");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "FeeRecords");

            migrationBuilder.DropColumn(
                name: "FeesPaid",
                table: "FeeRecords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "FeeRecords",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "FeeRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "FeesPaid",
                table: "FeeRecords",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
