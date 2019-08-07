using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll
{
    public interface IDashboardService
    {
        Task<List<DashboardMissionModel>> GetUserMissions(string userId);
        Task<ObservableCollection<PartialOrganizationModel>> GetUserOrganizations(string userId);
        Task<List<EventModel>> GetUserEvents(string userId);
        Task<UserModel> GetCurrentUser(string userId);
        Task<DashboardCountModel> GetIndicators(string userId);
        Task<int> GetMissionCountFromOrganization(string orgaId);
        Task<List<DashboardCustomerModel>> GetUserCustomers(string userId);
        Task<ActivityModel> GetCurrentActivity(DateTime? endDate = null);
        Task<ActivityModel> GetCurrentMissionActivity(string missionId);
        Task HideEvent(EventModel eventModel);
        Task<int> GetUserOrganizationCount(string id);
        Task<int> GetUserMissionCount(string id);
    }
}
