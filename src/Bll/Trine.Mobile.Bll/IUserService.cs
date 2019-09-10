using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll
{
    public interface IUserService
    {
        Task<List<UserModel>> SearchUsers(string email = null, string firstname = null, string lastname = null, string companyName = null);
        Task<UserModel> UpdateUser(UserModel user);
        Task<List<UserModel>> GetAllUsers();
        Task DeleteUser(string id);
        Task<UserModel> UploadProfilePicture(Stream stream, UserModel user);
    }
}
