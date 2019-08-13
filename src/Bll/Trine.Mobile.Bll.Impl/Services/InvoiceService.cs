using AutoMapper;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trine.Mobile.Bll.Impl.Services.Base;
using Trine.Mobile.Dal.Swagger;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll.Impl.Services
{
    public class InvoiceService : ServiceBase, IInvoiceService
    {
        private const string _apiVersion = "1.0";

        public InvoiceService(IMapper mapper, IGatewayRepository gatewayRepository, ILogger logger) : base(mapper, gatewayRepository, logger)
        {
        }

        public async Task<List<InvoiceModel>> GetFromMissionAsync(string missionId, int? quantity)
        {
            try
            {
                var invoices = await _gatewayRepository.ApiInvoicesMissionsByMissionIdGetAsync(missionId, quantity, _apiVersion);
                return _mapper.Map<List<InvoiceModel>>(invoices);
            }
            catch (ApiException dalExc)
            {
                throw dalExc;
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public async Task<List<InvoiceModel>> GetFromOrganizationAsync(string orgaId, int? quantity)
        {
            try
            {
                var invoices = await _gatewayRepository.ApiInvoicesOrganizationsByOrganizationIdGetAsync(orgaId, quantity, _apiVersion);
                return _mapper.Map<List<InvoiceModel>>(invoices);
            }
            catch (ApiException dalExc)
            {
                throw dalExc;
            }
            catch (Exception exc)
            {
                throw;
            }
        }
    }
}
