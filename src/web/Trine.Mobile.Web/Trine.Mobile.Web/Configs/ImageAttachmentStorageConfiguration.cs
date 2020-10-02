using Microsoft.Extensions.Configuration;
using Trine.Mobile.Dal.Configuration;

namespace Trine.Mobile.Web.Configs
{
    internal class ImageAttachmentStorageConfiguration : IImageAttachmentStorageConfiguration
    {
        private readonly IConfigurationSection _configuration;
        public ImageAttachmentStorageConfiguration(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("AzureBlob");
        }

#if DEBUG
        public string ConnectionString => _configuration.GetValue("ConnectionString", "UseDevelopmentStorage=true");
#else
        public string ConnectionString => _configuration.GetValue<string>("ConnectionString");
#endif
        public string Container => _configuration.GetValue("Container", "uploads");
    }
}
