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
    public class MissionService : ServiceBase, IMissionService
    {
        public MissionService(IMapper mapper, IGatewayRepository gatewayRepository, ILogger logger) : base(mapper, gatewayRepository, logger)
        {
        }

        public async Task<MissionModel> ActivateMissionAsync(MissionModel missionModel)
        {
            try
            {
                missionModel.Status = MissionModel.StatusEnum.CONFIRMED;
                var mission = _mapper.Map<MissionModel>(await _gatewayRepository.ApiMissionsPutAsync(missionModel.Id, _mapper.Map<Mission>(missionModel)));
                return mission;
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

        public async Task<MissionModel> CancelMissionAsync(string id)
        {
            try
            {
                var mission = _mapper.Map<MissionModel>(await _gatewayRepository.ApiMissionsByIdPutAsync(id));
                return mission;
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

        public async Task<MissionModel> CreateMission(CreateMissionRequestModel createMissionRequestModel)
        {
            try
            {
                var mission = _mapper.Map<MissionModel>(await _gatewayRepository.ApiMissionsPostAsync(_mapper.Map<CreateMissionRequest>(createMissionRequestModel)));
                return mission;
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

        public async Task<MissionModel> GetById(string id)
        {
            try
            {
                var mission = await _gatewayRepository.ApiMissionsByIdGetAsync(id);
                return _mapper.Map<MissionModel>(mission);
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

        public async Task<FrameContractModel> GetContractPreview(CreateMissionRequestModel createMissionRequestModel)
        {
            try
            {
                var contract = await _gatewayRepository.ApiMissionsContractPreviewPostAsync(_mapper.Map<CreateMissionRequest>(createMissionRequestModel));
                return _mapper.Map<FrameContractModel>(contract);
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

        public async Task<List<MissionModel>> GetFromOrganization(string id)
        {
            try
            {
                var missions = await _gatewayRepository.ApiMissionsOrganizationsByIdGetAsync(id);
                return _mapper.Map<List<MissionModel>>(missions);
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

        public async Task<List<ActivityModel>> GetMissionActivity(string missionId)
        {
            try
            {
                var activities = await _gatewayRepository.ApiActivitiesMissionsByMissionIdGetAsync(missionId);
                return _mapper.Map<List<ActivityModel>>(activities);
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

        public async Task<MissionModel> GetMissionById(string id)
        {
            try
            {
                var mission = await _gatewayRepository.ApiMissionsByIdGetAsync(id);
                return _mapper.Map<MissionModel>(mission);
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

        public async Task<List<MissionModel>> GetUserMissions(string userId)
        {
            try
            {
                var missions = await _gatewayRepository.ApiMissionsUsersByIdGetAsync(userId);
                return _mapper.Map<List<MissionModel>>(missions);
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
