using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories.Migrations
{
    public partial class AddSellerIdAndRemoveValidationCodeHash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "validation_code_hash",
                schema: "smart_vouchers",
                table: "voucher");

            migrationBuilder.AddColumn<Guid>(
                name: "seller_id",
                schema: "smart_vouchers",
                table: "voucher",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "seller_id",
                schema: "smart_vouchers",
                table: "voucher");

            migrationBuilder.AddColumn<string>(
                name: "validation_code_hash",
                schema: "smart_vouchers",
                table: "voucher",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
