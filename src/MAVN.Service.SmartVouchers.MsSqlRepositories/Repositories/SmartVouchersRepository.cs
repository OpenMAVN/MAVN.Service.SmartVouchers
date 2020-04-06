using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Common.MsSql;
using MAVN.Service.SmartVouchers.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories.Repositories
{
    public class SmartVouchersRepository : ISmartVouchersRepository
    {
        private readonly IDbContextFactory<SmartVouchersContext> _contextFactory;

        public SmartVouchersRepository(IDbContextFactory<SmartVouchersContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        
    }
}
