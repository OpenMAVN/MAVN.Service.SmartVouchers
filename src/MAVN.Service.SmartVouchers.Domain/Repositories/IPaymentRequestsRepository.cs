using System;
using System.Threading.Tasks;

namespace MAVN.Service.SmartVouchers.Domain.Repositories
{
    public interface IPaymentRequestsRepository
    {
        Task CreatePaymentRequestAsync(Guid paymentRequestId, string voucherShortCode);
        Task<string> GetVoucherShortCodeAsync(Guid paymentRequestId);
        Task<Guid?> PaymentRequestExistsAsync(string voucherShortCode);
    }
}
