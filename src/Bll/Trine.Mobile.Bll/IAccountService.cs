using System.Threading.Tasks;
using Trine.Mobile.Model;

namespace Trine.Mobile.Bll
{
    public interface IAccountService
    {
        Task<string> Login(string email, string password);
        Task<string> RegisterCompany(RegisterCompanyModel model);
        Task<string> RegisterUser(RegisterUserModel model);
        Task RecoverPasswordAsync(PasswordUpdateModel model);
    }
}
