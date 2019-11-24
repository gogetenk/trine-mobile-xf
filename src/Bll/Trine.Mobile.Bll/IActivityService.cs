using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll
{
    public interface IActivityService
    {
        Task<ActivityModel> CreateActivity(string missionId, DateTime date);
        Task<ActivityModel> GenerateNewActivityReport();
        Task<UserModel> GetActivityCustomer(ActivityModel activity);
        Task<ActivityModel> GetFromMissionAndMonth(string missionId, DateTime dateTime);
        Task<UserModel> GetActivityConsultant(ActivityModel activity);
        Task<ActivityModel> SignActivityReport(UserModel customer, ActivityModel activity, Stream imageStream);
        Task<ActivityModel> SaveActivityReport(ActivityModel activityModel);
        Task<ActivityModel> GetById(string activityId);
        Task<ActivityModel> UpdateActivity(ActivityModel currentActivity);
        Task DeleteActivity(string id);
        Task<ObservableCollection<ActivityModel>> GetFromMission(string missionId);
        Task RefuseActivity(ActivityModel activityModel);
        List<DateTime> GetMissionPeriods(MissionModel mission);
        Task<List<ActivityModel>> GetFromUser(string id, ActivityStatusEnum status);
    }
}
