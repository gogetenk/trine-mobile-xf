using System.Collections.Generic;
using System.Threading.Tasks;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll
{
    public interface IMissionService
    {
        Task<List<MissionModel>> GetUserMissions(string userId);
        Task<FrameContractModel> GetContractPreview(CreateMissionRequestModel createMissionRequestDto);
        Task<MissionModel> CreateMission(CreateMissionRequestModel createMissionRequestModel);
        Task<List<ActivityModel>> GetMissionActivity(string missionId);
        Task<MissionModel> GetById(string id);
        Task<List<MissionModel>> GetFromOrganization(string id);
        Task<MissionModel> GetMissionById(string id);
        Task<MissionModel> CancelMissionAsync(string id);
        Task<MissionModel> ActivateMissionAsync(MissionModel missionModel);
        Task<MissionModel> GetConsultantCurrentMission(string id);
    }
}
