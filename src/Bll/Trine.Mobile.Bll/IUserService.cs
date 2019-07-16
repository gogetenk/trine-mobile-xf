using System.Collections.Generic;
using System.Threading.Tasks;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll
{
    public interface IUserService
    {
        Task<List<UserModel>> SearchUsers(string email = null, string firstname = null, string lastname = null, string companyName = null);
        Task<List<UserModel>> GetAllUsers();
        Task DeleteUser(string id);
    }
}
