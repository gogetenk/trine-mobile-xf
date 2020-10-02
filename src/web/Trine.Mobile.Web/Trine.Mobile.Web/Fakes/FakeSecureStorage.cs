using System.Collections.Generic;
using System.Threading.Tasks;
using Trine.Mobile.Bll;

namespace Trine.Mobile.Web.Fakes
{
    public class FakeSecureStorage : ISecureStorage
    {
        private readonly Dictionary<string, string> _storage = new Dictionary<string, string>();

        public async Task<string> GetAsync(string key)
        {
            return _storage[key];
        }

        public async Task SetAsync(string key, string value)
        {
            _storage[key] = value;
        }
    }
}
