using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Prism.Logging;
using Sogetrel.Sinapse.Framework.Exceptions;
using Trine.Mobile.Bll.Impl.Services.Base;
using Trine.Mobile.Bll.Impl.Settings;
using Trine.Mobile.Dal.Swagger;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll.Impl.Services
{
    public class OrganizationService : ServiceBase, IOrganizationService
    {
        private const string _organizationApiVersion = "1.0";

        public OrganizationService(IMapper mapper, IGatewayRepository gatewayRepository, ILogger logger) : base(mapper, gatewayRepository, logger)
        {
        }

        public async Task<string> CreateOrganization(string organizationName, string iconUrl)
        {
            var orga = new OrganizationModel()
            {
                Created = DateTime.UtcNow,
                Icon = iconUrl,
                Name = organizationName,
                OwnerId = AppSettings.CurrentUser.Id,
                Members = new List<OrganizationMemberModel>()
                {
                    new OrganizationMemberModel()
                    {
                        JoinedAt = DateTime.UtcNow,
                        Role = OrganizationMemberModel.RoleEnum.ADMIN,
                        UserId = AppSettings.CurrentUser.Id
                    }
                }
            };

            var addedOrga = await _gatewayRepository.ApiOrganizationsPostAsync(_mapper.Map<Organization>(orga));
            if (addedOrga is null)
                throw new TechnicalException("An error occured while creating this organization");

            return addedOrga.Id;
        }

        public async Task<string> JoinOrganization(Guid codeGuid)
        {
            try
            {
                // We get the concerned invitation
                var invite = await _gatewayRepository.ApiOrganizationsInvitesByCodeGetAsync(codeGuid);
                if (invite is null)
                    throw new BusinessException("L'organisation ou l'invitation n'existe plus.");

                // On ajoute l'utilisateur courant à l'orga concernée
                var registerUserRequest = new RegisterUserRequest()
                {
                    Email = AppSettings.CurrentUser.Mail,
                    FirstName = AppSettings.CurrentUser.Firstname,
                    LastName = AppSettings.CurrentUser.Lastname,
                    GlobalRole = RegisterUserRequestGlobalRole._0, // TODO : gérer ça
                    InvitationCode = invite.Code.ToString()
                };
                var joinOrgaRequest = new JoinOrganizationRequest()
                {
                    AccountData = registerUserRequest,
                    Code = invite.Code.ToString()
                };

                var member = _mapper.Map<OrganizationMemberModel>(await _gatewayRepository.ApiOrganizationsJoinPostAsync(joinOrgaRequest, _organizationApiVersion));
                if (member is null)
                    throw new TechnicalException($"An error occured while creating the member in organization : [{codeGuid.ToString()}]");

                return invite.OrganizationId;
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

        public async Task<InviteModel> GetOrganizationInvite(string organizationId, string missionId)
        {
            try
            {
                var request = new CreateInvitationRequest()
                {
                    InviterId = AppSettings.CurrentUser.Id,
                    MissionId = missionId
                };

                var invite = _mapper.Map<InviteModel>(await _gatewayRepository.ApiOrganizationsByOrganizationIdInvitesPostAsync(organizationId, request));

                if (invite is null)
                    throw new BusinessException("Une erreur s'est produite lors de la création de cette invitation.");
                //if (invite.IsRevoked)
                //    throw new BusinessException("Cette invitation est révoquée et ne peut plus être utilisée.");
                if (invite.Expires <= DateTime.UtcNow)
                    throw new BusinessException("Cette invitation est expirée et ne peut plus être utilisée.");

                return invite;
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

        public async Task<List<UserModel>> GetOrganizationMembers(string organizationId)
        {
            try
            {
                var members = _mapper.Map<List<UserModel>>(await _gatewayRepository.ApiOrganizationsByOrganizationIdMembersGetAsync(organizationId));
                if (members is null)
                    return new List<UserModel>();

                return members;
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

        public async Task<OrganizationMemberModel> GetMember(string organizationId, string memberId)
        {
            try
            {
                var member = _mapper.Map<OrganizationMemberModel>(await _gatewayRepository.ApiOrganizationsByOrganizationIdMembersByUserIdGetAsync(organizationId, memberId));
                if (member is null)
                    throw new BusinessException("Une erreur s'est produite lors de la récupération du membre.");

                return member;
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

        public async Task<OrganizationMemberModel> UpdateMember(string organizationId, OrganizationMemberModel organizationMemberModel)
        {
            try
            {
                var memberToUpdate = _mapper.Map<OrganizationMember>(organizationMemberModel);
                var member = _mapper.Map<OrganizationMemberModel>(await _gatewayRepository.ApiOrganizationsByOrganizationIdMembersPatchAsync(organizationId, memberToUpdate));
                if (member is null)
                    throw new BusinessException("Une erreur s'est produite lors de la mise à jour du membre.");

                return member;
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

        public async Task RemoveMember(string organizationId, string memberId)
        {
            try
            {
                await _gatewayRepository.ApiOrganizationsByOrganizationIdMembersByUserIdDeleteAsync(organizationId, memberId);
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

        public async Task DeleteOrganization(string id)
        {
            try
            {
                await _gatewayRepository.ApiActivitiesByIdDeleteAsync(id);
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

        public async Task<PartialOrganizationModel> GetById(string id)
        {
            try
            {
                var orga = await _gatewayRepository.ApiOrganizationsByOrganizationIdGetAsync(id);
                var partialOrga = new PartialOrganizationModel()
                {
                    Id = orga.Id,
                    Icon = orga.Icon,
                    //IsOwner = (AppSettings.CurrentUser.Id == orga.OwnerId),
                    Name = orga.Name,
                    //UserRole = (RoleEnum)orga.Members.FirstOrDefault(x => x.UserId == AppSettings.CurrentUser.Id)?.Role
                };
                return partialOrga;
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

        public async Task<InviteModel> SendInvitation(string orgaId, CreateInvitationRequestModel request)
        {
            try
            {
                var invite = await _gatewayRepository.ApiOrganizationsByOrganizationIdInvitesPostAsync(orgaId, _mapper.Map<CreateInvitationRequest>(request));
                return _mapper.Map<InviteModel>(invite.FirstOrDefault());
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

        public async Task<List<InviteModel>> GetInvites(string id)
        {
            try
            {
                var invite = await _gatewayRepository.ApiOrganizationsByOrganizationIdInvitesGetAsync(id);
                return _mapper.Map<List<InviteModel>>(invite).OrderByDescending(x => x.Created).ToList();
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
