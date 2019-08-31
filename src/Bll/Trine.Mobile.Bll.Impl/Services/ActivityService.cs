using AutoMapper;
using Prism.Logging;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Trine.Mobile.Bll.Impl.Services.Base;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Dal.Swagger;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll.Impl.Services
{
    public class ActivityService : ServiceBase, IActivityService
    {
        private const string _activityApiVersion = "1.0";
        private const string _userApiVersion = "1.0";

        public ActivityService(IMapper mapper, IGatewayRepository gatewayRepository, ILogger logger) : base(mapper, gatewayRepository, logger)
        {
        }

        public async Task<ActivityModel> GenerateNewActivityReport(string missionId)
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

        public async Task<UserModel> GetActivityCustomer(ActivityModel activity)
        {
            try
            {
                return _mapper.Map<UserModel>(await _gatewayRepository.ApiAccountsUsersByIdGetAsync(activity.Customer.Id, _userApiVersion));
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

        public async Task<UserModel> GetActivityConsultant(ActivityModel activity)
        {
            try
            {
                return _mapper.Map<UserModel>(await _gatewayRepository.ApiAccountsUsersByIdGetAsync(activity.Consultant.Id, _userApiVersion));
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

        public async Task<ActivityModel> SignActivityReport(UserModel user, ActivityModel activity)
        {
            try
            {
                if (user is null || activity is null)
                    throw new TechnicalException("Customer/Consultant or activity cannot be null while signing.");

                var result = _mapper.Map<ActivityModel>(await _gatewayRepository.ApiActivitiesSignPatchAsync(activity.Id, user.Id, _activityApiVersion));
                if (result == null)
                    throw new TechnicalException("An error occured while signing. Please proceed again.");

                return result;
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

        public async Task<ActivityModel> SaveActivityReport(ActivityModel activityModel)
        {
            try
            {
                var request = new CreateActivityRequest();
                request.MissionId = activityModel.MissionId;
                request.UserId = AppSettings.CurrentUser.Id;
                request.Days = _mapper.Map<List<GridDay>>(activityModel.Days);
                var newActivity = await _gatewayRepository.ApiActivitiesPostAsync(request, _userApiVersion);
                return _mapper.Map<ActivityModel>(newActivity);
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

        public async Task<ActivityModel> GetById(string activityId)
        {
            try
            {
                var newActivity = await _gatewayRepository.ApiActivitiesByActivityIdGetAsync(activityId, _userApiVersion);
                return _mapper.Map<ActivityModel>(newActivity);
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

        public async Task UpdateActivity(ActivityModel currentActivity)
        {
            try
            {
                var result = _mapper.Map<ActivityModel>(await _gatewayRepository.ApiActivitiesPutAsync(currentActivity.Id, _mapper.Map<Activity>(currentActivity), _activityApiVersion));
                if (result == null)
                    throw new TechnicalException("An error occured while updating the activity report. Please proceed again.");
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

        public async Task DeleteActivity(string id)
        {
            try
            {
                await _gatewayRepository.ApiActivitiesByIdDeleteAsync(id, _activityApiVersion);
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

        public async Task<ObservableCollection<ActivityModel>> GetFromMission(string missionId)
        {
            try
            {
                return _mapper.Map<ObservableCollection<ActivityModel>>(await _gatewayRepository.ApiActivitiesMissionsByMissionIdGetAsync(missionId, _activityApiVersion));
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

        public async Task RefuseActivity(ActivityModel activityModel)
        {
            try
            {
                await _gatewayRepository.ApiActivitiesByActivityIdRequestChangePostAsync(activityModel.Id, AppSettings.CurrentUser.Id, _activityApiVersion);
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
