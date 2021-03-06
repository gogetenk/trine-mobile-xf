﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll
{
    public interface IOrganizationService
    {
        Task<string> JoinOrganization(Guid codeGuid);
        Task<OrganizationModel> CreateOrganization(string organizationName, string iconUrl);
        Task<InviteModel> GetOrganizationInvite(string organizationId, string missionId);
        Task<List<UserModel>> GetOrganizationMembers(string organizationId);
        Task<OrganizationMemberModel> GetMember(string organizationId, string memberId);
        Task<OrganizationMemberModel> UpdateMember(string organizationId, OrganizationMemberModel organizationMemberModel);
        Task RemoveMember(string organizationId, string memberId);
        Task DeleteOrganization(string id);
        Task<PartialOrganizationModel> GetById(string id);
        Task<InviteModel> SendInvitation(string id, CreateInvitationRequestModel request);
        Task<List<InviteModel>> GetInvites(string v);
    }
}
