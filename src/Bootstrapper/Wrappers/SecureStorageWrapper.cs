using System;
using System.Threading.Tasks;
using Trine.Mobile.Bll;
using Xamarin.Essentials;

namespace Trine.Mobile.Bootstrapper.Wrappers
{
    public class SecureStorageWrapper : ISecureStorage
    {
        private static Lazy<SecureStorageWrapper> Implementation = new Lazy<SecureStorageWrapper>();

        public static ISecureStorage Current
        {
            get
            {
                if (Implementation.Value == null)
                {
                    Implementation = new Lazy<SecureStorageWrapper>();
                }
                return Implementation.Value;
            }
        }

        public async Task<string> GetAsync(string key)
        {
            return await SecureStorage.GetAsync(key);
        }

        public async Task SetAsync(string key, string value)
        {
            await SecureStorage.SetAsync(key, value);
        }
    }
}
