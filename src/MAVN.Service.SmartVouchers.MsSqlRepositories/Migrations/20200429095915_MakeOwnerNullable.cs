using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories.Migrations
{
    public partial class MakeOwnerNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "owner_id",
                schema: "smart_vouchers",
                table: "voucher",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "owner_id",
                schema: "smart_vouchers",
                table: "voucher",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
