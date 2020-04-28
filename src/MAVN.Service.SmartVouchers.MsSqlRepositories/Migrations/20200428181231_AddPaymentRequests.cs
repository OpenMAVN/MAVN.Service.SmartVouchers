using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories.Migrations
{
    public partial class AddPaymentRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "partner_id",
                schema: "smart_vouchers",
                table: "campaign",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "payment_request",
                schema: "smart_vouchers",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    short_code = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_request", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payment_request",
                schema: "smart_vouchers");

            migrationBuilder.AlterColumn<string>(
                name: "partner_id",
                schema: "smart_vouchers",
                table: "campaign",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid));
        }
    }
}
