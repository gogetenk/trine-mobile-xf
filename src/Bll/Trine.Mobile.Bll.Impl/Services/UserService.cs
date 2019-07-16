using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Prism.Logging;
using Trine.Mobile.Bll.Impl.Services.Base;
using Trine.Mobile.Dal.Swagger;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll.Impl.Services
{
    public class UserService : ServiceBase, IUserService
    {
        public UserService(IMapper mapper, IGatewayRepository gatewayRepository, ILogger logger) : base(mapper, gatewayRepository, logger)
        {
        }

        public async Task DeleteUser(string id)
        {
            try
            {
                await _gatewayRepository.ApiUsersDeleteAsync(id);
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

        public async Task<List<UserModel>> GetAllUsers()
        {
            try
            {
                var users = await _gatewayRepository.ApiUsersGetAsync();
                var mappedUsers = _mapper.Map<List<UserModel>>(users);
                return mappedUsers;
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

        public async Task<List<UserModel>> SearchUsers(string email = null, string firstname = null, string lastname = null, string companyName = null)
        {
            try
            {
                var users = await _gatewayRepository.ApiUsersSearchGetAsync(email, firstname, lastname, companyName);
                return _mapper.Map<List<UserModel>>(users);
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
