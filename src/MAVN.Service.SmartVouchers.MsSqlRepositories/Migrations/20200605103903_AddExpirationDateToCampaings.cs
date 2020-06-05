using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories.Migrations
{
    public partial class AddExpirationDateToCampaings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "expiration_date",
                schema: "smart_vouchers",
                table: "campaign",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "expiration_date",
                schema: "smart_vouchers",
                table: "campaign");
        }
    }
}
