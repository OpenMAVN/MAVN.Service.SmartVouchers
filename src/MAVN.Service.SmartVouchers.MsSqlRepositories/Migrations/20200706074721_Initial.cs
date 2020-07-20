using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "smart_vouchers");

            migrationBuilder.CreateTable(
                name: "campaign",
                schema: "smart_vouchers",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    vouchers_total_count = table.Column<int>(nullable: false),
                    bought_vouchers_count = table.Column<int>(nullable: false),
                    voucher_price = table.Column<decimal>(nullable: false),
                    currency = table.Column<string>(nullable: false),
                    partner_id = table.Column<Guid>(nullable: false),
                    from_date = table.Column<DateTime>(nullable: false),
                    to_date = table.Column<DateTime>(nullable: true),
                    expiration_date = table.Column<DateTime>(nullable: true),
                    creation_date = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(nullable: false),
                    state = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campaign", x => x.id);
                });

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

            migrationBuilder.CreateTable(
                name: "voucher",
                schema: "smart_vouchers",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    short_code = table.Column<string>(nullable: true),
                    seller_id = table.Column<Guid>(nullable: true),
                    campaign_id = table.Column<Guid>(nullable: false),
                    status = table.Column<short>(nullable: false),
                    owner_id = table.Column<Guid>(nullable: true),
                    purchase_date = table.Column<DateTime>(nullable: true),
                    redemption_date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_voucher", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "campaign_content",
                schema: "smart_vouchers",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    campaign_id = table.Column<Guid>(nullable: false),
                    content_type = table.Column<int>(nullable: false),
                    language = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campaign_content", x => x.id);
                    table.ForeignKey(
                        name: "FK_campaign_content_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalSchema: "smart_vouchers",
                        principalTable: "campaign",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "voucher_validation",
                schema: "smart_vouchers",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    voucher_id = table.Column<long>(nullable: false),
                    validation_code = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_voucher_validation", x => x.id);
                    table.ForeignKey(
                        name: "FK_voucher_validation_voucher_voucher_id",
                        column: x => x.voucher_id,
                        principalSchema: "smart_vouchers",
                        principalTable: "voucher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_campaign_content_campaign_id",
                schema: "smart_vouchers",
                table: "campaign_content",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_request_short_code",
                schema: "smart_vouchers",
                table: "payment_request",
                column: "short_code");

            migrationBuilder.CreateIndex(
                name: "IX_voucher_campaign_id",
                schema: "smart_vouchers",
                table: "voucher",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_voucher_owner_id",
                schema: "smart_vouchers",
                table: "voucher",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_voucher_short_code",
                schema: "smart_vouchers",
                table: "voucher",
                column: "short_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_voucher_validation_voucher_id",
                schema: "smart_vouchers",
                table: "voucher_validation",
                column: "voucher_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "campaign_content",
                schema: "smart_vouchers");

            migrationBuilder.DropTable(
                name: "payment_request",
                schema: "smart_vouchers");

            migrationBuilder.DropTable(
                name: "voucher_validation",
                schema: "smart_vouchers");

            migrationBuilder.DropTable(
                name: "campaign",
                schema: "smart_vouchers");

            migrationBuilder.DropTable(
                name: "voucher",
                schema: "smart_vouchers");
        }
    }
}
