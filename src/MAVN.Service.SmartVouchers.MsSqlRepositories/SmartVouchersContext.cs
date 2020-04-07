using System.Data.Common;
using JetBrains.Annotations;
using Lykke.Common.MsSql;
using MAVN.Service.SmartVouchers.MsSqlRepositories.Entities;
using MAVN.Service.SmartVouchers.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories
{
    public class SmartVouchersContext : MsSqlContext
    {
        private const string Schema = "smart_vouchers";

        public DbSet<VoucherCampaignEntity> VoucherCampaigns { get; set; }
        public DbSet<VoucherCampaignContentEntity> CampaignsContents { get; set; }
        public DbSet<VoucherEntity> Vouchers { get; set; }
        public DbSet<VoucherValidationEntity> VoucherValidations { get; set; }

        // empty constructor needed for EF migrations
        [UsedImplicitly]
        public SmartVouchersContext()
            : base(Schema)
        {
        }

        public SmartVouchersContext(string connectionString, bool isTraceEnabled)
            : base(Schema, connectionString, isTraceEnabled)
        {
        }

        //Needed constructor for using InMemoryDatabase for tests
        public SmartVouchersContext(DbContextOptions options)
            : base(Schema, options)
        {
        }

        public SmartVouchersContext(DbConnection dbConnection)
            : base(Schema, dbConnection)
        {
        }

        protected override void OnLykkeModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VoucherCampaignEntity>()
                .Property(o => o.State).HasConversion(new EnumToNumberConverter<CampaignState, short>());

            modelBuilder.Entity<VoucherCampaignEntity>()
                .HasMany(e => e.LocalizedContents)
                .WithOne()
                .HasForeignKey(e => e.CampaignId);

            modelBuilder.Entity<VoucherEntity>()
                .HasOne(o => o.Validation)
                .WithOne()
                .HasForeignKey<VoucherValidationEntity>(o => o.VoucherId);

            modelBuilder.Entity<VoucherEntity>()
                .Property(o => o.Status)
                .HasConversion(new EnumToNumberConverter<VoucherStatus, short>());

            modelBuilder.Entity<VoucherEntity>()
                .HasIndex(o => o.CampaignId)
                .IsUnique(false);

            modelBuilder.Entity<VoucherEntity>()
                .HasIndex(o => o.OwnerId)
                .IsUnique(false);

            modelBuilder.Entity<VoucherEntity>()
                .HasIndex(o => o.ShortCode)
                .IsUnique();

            modelBuilder.Entity<VoucherValidationEntity>()
                .HasIndex(o => o.VoucherId)
                .IsUnique();
        }
    }
}
