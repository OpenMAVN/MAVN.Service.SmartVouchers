using System.Threading.Tasks;

namespace MAVN.Service.SmartVouchers.Domain.Services
{
    public interface INotificationsService
    {
        Task PublishVoucherSuccessfullyRedeemed(string customerId, string partnerName, string voucherShortCode);
    }
}
