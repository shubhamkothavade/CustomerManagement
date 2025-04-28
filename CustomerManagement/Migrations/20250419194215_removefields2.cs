using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerManagement.Migrations
{
    /// <inheritdoc />
    public partial class removefields2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeesPaid",
                table: "FeeRecords");

            migrationBuilder.DropColumn(
                name: "PendingFees",
                table: "FeeRecords");

            migrationBuilder.DropColumn(
                name: "PendingFees",
                table: "FeeHistories");

            migrationBuilder.DropColumn(
                name: "TotalFees",
                table: "FeeHistories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FeesPaid",
                table: "FeeRecords",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PendingFees",
                table: "FeeRecords",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PendingFees",
                table: "FeeHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalFees",
                table: "FeeHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
