﻿using System;
using System.Threading.Tasks;
using MAVN.Persistence.PostgreSQL.Legacy;
using MAVN.Service.SmartVouchers.Domain.Repositories;
using MAVN.Service.SmartVouchers.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.SmartVouchers.MsSqlRepositories.Repositories
{
    public class PaymentRequestsRepository : IPaymentRequestsRepository
    {
        private readonly IDbContextFactory<SmartVouchersContext> _contextFactory;

        public PaymentRequestsRepository(IDbContextFactory<SmartVouchersContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreatePaymentRequestAsync(Guid paymentRequestId, string voucherShortCode)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var paymentRequest = new PaymentRequestEntity
                {
                    Id = paymentRequestId,
                    VoucherShortCode = voucherShortCode,
                };

                context.PaymentRequests.Add(paymentRequest);

                await context.SaveChangesAsync();
            }
        }

        public async Task<string> GetVoucherShortCodeAsync(Guid paymentRequestId)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var result = await context.PaymentRequests.FindAsync(paymentRequestId);

                return result?.VoucherShortCode;
            }
        }

        public async Task<Guid?> PaymentRequestExistsAsync(string voucherShortCode)
        {
            using (var context = _contextFactory.CreateDataContext())
            {
                var result = await context.PaymentRequests.FirstOrDefaultAsync(p => p.VoucherShortCode == voucherShortCode);

                return result?.Id;
            }
        }
    }
}
