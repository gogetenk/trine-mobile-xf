using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sogetrel.Sinapse.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public async Task<ActivityModel> CreateActivity(string missionId, DateTime date)
        {
            try
            {
                return _mapper.Map<ActivityModel>(await _gatewayRepository.ApiActivitiesPostAsync(missionId, date, _activityApiVersion));
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

        public async Task<ActivityModel> GenerateNewActivityReport()
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

        public async Task<ActivityModel> SignActivityReport(UserModel user, ActivityModel activity, Stream imageStream)
        {
            try
            {
                if (user is null || activity is null)
                    throw new TechnicalException("Customer/Consultant or activity cannot be null while signing.");

                //var result = _mapper.Map<ActivityModel>(await _gatewayRepository.ApiActivitiesSignPatchAsync(activity.Id, user.Id, _activityApiVersion));
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppSettings.AccessToken.AccessToken);
                MultipartFormDataContent form = new MultipartFormDataContent();
                HttpContent content = new StringContent("fileToUpload");
                form.Add(content, "fileToUpload");
                content = new StreamContent(imageStream);
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = "signature.png"
                };
                form.Add(content);
                var response = await client.PostAsync($"{AppSettings.ApiUrls["GatewayApi"]}/api/activities/sign?activityId={activity?.Id}&userId={user?.Id}", form);
                if (!response.IsSuccessStatusCode)
                    throw new BusinessException("Une erreur s'est produite lors de la signature.");

                var result = await response.Content.ReadAsStringAsync();
                if (result == null)
                    throw new TechnicalException("An error occured while signing. Please proceed again.");

                return JsonConvert.DeserializeObject<ActivityModel>(result);
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
                var newActivity = await _gatewayRepository.ApiActivitiesPutAsync(activityModel.Id, _mapper.Map<Activity>(activityModel), _userApiVersion);
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

        public async Task<ActivityModel> UpdateActivity(ActivityModel currentActivity)
        {
            try
            {
                var result = _mapper.Map<ActivityModel>(await _gatewayRepository.ApiActivitiesPutAsync(currentActivity.Id, _mapper.Map<Activity>(currentActivity), _activityApiVersion));
                if (result == null)
                    throw new TechnicalException("An error occured while updating the activity report. Please proceed again.");
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

        public async Task<ActivityModel> GetFromMissionAndMonth(string missionId, DateTime dateTime)
        {
            try
            {
                return _mapper.Map<ActivityModel>(await _gatewayRepository.ApiActivitiesMissionsByMissionIdByDateGetAsync(missionId, dateTime, _activityApiVersion));
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

        public List<DateTime> GetMissionPeriods(MissionModel mission)
        {
            var startDt = mission.StartDate;
            var endDt = mission.EndDate;

            var diff = Enumerable.Range(0, 13)
                        .Select(a => startDt.AddMonths(a))
                        .TakeWhile(a => a <= endDt)
                        .ToList();

            return diff;
        }

        public async Task<List<ActivityModel>> GetFromUser(string id, ActivityStatusEnum status)
        {
            try
            {
                return _mapper.Map<List<ActivityModel>>(await _gatewayRepository.ApiActivitiesUsersByUserIdGetAsync(id, (int)status, _activityApiVersion));
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
