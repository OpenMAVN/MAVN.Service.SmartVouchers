using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories.Migrations
{
    public partial class AddIndexByShortCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "short_code",
                schema: "smart_vouchers",
                table: "payment_request",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_payment_request_short_code",
                schema: "smart_vouchers",
                table: "payment_request",
                column: "short_code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_payment_request_short_code",
                schema: "smart_vouchers",
                table: "payment_request");

            migrationBuilder.AlterColumn<string>(
                name: "short_code",
                schema: "smart_vouchers",
                table: "payment_request",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
