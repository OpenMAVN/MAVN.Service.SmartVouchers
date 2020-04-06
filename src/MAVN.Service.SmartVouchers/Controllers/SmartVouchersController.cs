using AutoMapper;
using MAVN.Service.SmartVouchers.Client;
using Microsoft.AspNetCore.Mvc;

namespace MAVN.Service.SmartVouchers.Controllers
{
    [Route("api/SmartVouchers")] // TODO fix route
    public class SmartVouchersController : Controller, ISmartVouchersApi
    {
        private readonly IMapper _mapper;

        public SmartVouchersController(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
