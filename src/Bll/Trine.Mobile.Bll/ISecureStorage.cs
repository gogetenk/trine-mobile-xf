using System.Threading.Tasks;

namespace Trine.Mobile.Bll
{
    public interface ISecureStorage
    {
        Task<string> GetAsync(string key);
        Task SetAsync(string key, string value);
    }
}
