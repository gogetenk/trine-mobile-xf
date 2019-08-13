using System.Collections.Generic;
using System.Threading.Tasks;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll
{
    public interface IInvoiceService
    {
        Task<List<InvoiceModel>> GetFromOrganizationAsync(string orgaId, int? quantity);
        Task<List<InvoiceModel>> GetFromMissionAsync(string missionId, int? quantity);
    }
}
