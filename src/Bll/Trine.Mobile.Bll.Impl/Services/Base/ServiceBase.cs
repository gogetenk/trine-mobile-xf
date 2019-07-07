using AutoMapper;
using Prism.Logging;

namespace Trine.Mobile.Bll.Impl.Services.Base
{
    public class ServiceBase
    {
        protected readonly Dal.Swagger.IGatewayRepository _gatewayRepository;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;

        public ServiceBase(IMapper mapper, Dal.Swagger.IGatewayRepository gatewayRepository, ILogger logger)
        {
            _mapper = mapper;
            _gatewayRepository = gatewayRepository;
            _logger = logger;
        }
    }
}
