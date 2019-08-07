using AutoMapper;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Trine.Mobile.Bll.Impl.Services.Base;
using Trine.Mobile.Dal.Swagger;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll.Impl.Services
{
    public class DashboardService : ServiceBase, IDashboardService
    {
        private readonly string _activityApiVersion = "1.0";

        public DashboardService(IMapper mapper, IGatewayRepository gatewayRepository, ILogger logger) : base(mapper, gatewayRepository, logger)
        {
        }

        public async Task<UserModel> GetCurrentUser(string userId)
        {
            try
            {
                var user = await _gatewayRepository.ApiAccountsUsersByIdGetAsync(userId);
                return _mapper.Map<UserModel>(user);
            }
            catch (ApiException dalExc)
            {
                throw dalExc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ActivityModel> GetCurrentActivity(DateTime? endDate = null)
        {
            try
            {
                return _mapper.Map<ActivityModel>(await _gatewayRepository.ApiActivitiesGenerateGetAsync(_activityApiVersion));
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

        public async Task<ActivityModel> GetCurrentMissionActivity(string missionId)
        {
            try
            {
                Activity values = new Activity();

                values = await _gatewayRepository.ApiActivitiesMissionsByMissionIdByDateGetAsync(missionId, DateTime.UtcNow, _activityApiVersion);
                return _mapper.Map<ActivityModel>(values);
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


        public async Task<List<DashboardMissionModel>> GetUserMissions(string userId)
        {
            try
            {
                List<DashboardMissionModel> values = new List<DashboardMissionModel>();
                values = _mapper.Map<List<DashboardMissionModel>>(await _gatewayRepository.ApiDashboardsMissionsUsersByUserIdGetAsync(userId));

                return values;
            }
            catch (ApiException dalExc)
            {
                throw dalExc;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ObservableCollection<PartialOrganizationModel>> GetUserOrganizations(string userId)
        {
            try
            {
                return _mapper.Map<ObservableCollection<PartialOrganizationModel>>(await _gatewayRepository.ApiDashboardsOrganizationsUsersByUserIdGetAsync(userId));
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

        public async Task<int> GetMissionCountFromOrganization(string orgaId)
        {
            try
            {
                var count = await _gatewayRepository.ApiDashboardsMissionsCountByOrganizationIdGetAsync(orgaId);
                return count;
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

        public async Task<List<EventModel>> GetUserEvents(string userId)
        {
            try
            {
                List<EventModel> events = new List<EventModel>();
                events = Mapper.Map<List<EventModel>>(await _gatewayRepository.ApiDashboardsEventsGetAsync(userId));
                return events;
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

        public async Task<DashboardCountModel> GetIndicators(string userId)
        {
            try
            {
                var indicators = await _gatewayRepository.ApiDashboardsGetAsync(userId);
                return _mapper.Map<DashboardCountModel>(indicators);
            }
            catch (ApiException dalExc)
            {
                throw dalExc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DashboardCustomerModel>> GetUserCustomers(string userId)
        {
            try
            {
                var customers = await _gatewayRepository.ApiDashboardsCustomersUsersByIdGetAsync(userId);
                return _mapper.Map<List<DashboardCustomerModel>>(customers);
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

        // Mark an event as read so it's no longer shown to the dashboard.
        public async Task HideEvent(EventModel eventModel)
        {
            try
            {
                eventModel.IsEnabled = false;
                await _gatewayRepository.ApiEventsEventsPatchAsync(eventModel.Id, _mapper.Map<Event>(eventModel));
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

        public async Task<int> GetUserOrganizationCount(string id)
        {
            try
            {
                return await _gatewayRepository.ApiDashboardsOrganizationsUsersByUserIdCountGetAsync(id);
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

        public async Task<int> GetUserMissionCount(string id)
        {
            try
            {
                return await _gatewayRepository.ApiDashboardsMissionsUsersByUserIdCountGetAsync(id);
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
