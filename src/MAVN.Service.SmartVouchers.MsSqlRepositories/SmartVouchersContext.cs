using System.Data.Common;
using JetBrains.Annotations;
using Lykke.Common.MsSql;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories
{
    public class SmartVouchersContext : MsSqlContext
    {
        private const string Schema = "smart_vouchers";

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
            // TODO put db entities models building code here
        }
    }
}
