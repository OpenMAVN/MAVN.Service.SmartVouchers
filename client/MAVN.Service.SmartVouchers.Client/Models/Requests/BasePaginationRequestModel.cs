using System.ComponentModel.DataAnnotations;

namespace MAVN.Service.SmartVouchers.Client.Models.Requests
{
    /// <summary>
    /// Model that contains information about pager's values
    /// </summary>
    public class BasePaginationRequestModel
    {
        /// <summary>
        /// Represents current page number
        /// </summary>
        [Range(1, int.MaxValue)]
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// TRepresents page size
        /// </summary>
        [Range(1, 1000)]
        public int PageSize { get; set; } = 500;
    }
}
