using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trine.Mobile.Dal.Configuration;

namespace Trine.Mobile.Dal.AzureBlobStorage.Repositories
{
    public class ImageAttachmentStorageRepository : IImageAttachmentStorageRepository
    {
        private readonly CloudBlobContainer _container;
        private readonly IImageAttachmentStorageConfiguration _storageConfig;

        public ImageAttachmentStorageRepository(IImageAttachmentStorageConfiguration storageConfig)
        {
            _storageConfig = storageConfig;

            // Create cloudstorage account by passing the storagecredentials
            CloudStorageAccount.TryParse("DefaultEndpointsProtocol=https;AccountName=trinestorage;AccountKey=8HRniOQOtpngEXFZ1rj8LenfhtyEttjpwAoNh4VXTvrARjG2HL+aAeIU3g3QDon9TM6DGz8RerFJvdgHNQa+mg==;EndpointSuffix=core.windows.net", out CloudStorageAccount storageAccount);
            // Create the blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();
            // Get reference to the blob container by passing the name by reading the value from the configuration (appsettings.json)
            _container = blobClient.GetContainerReference("profilepictures");
        }

        public async Task<Uri> UploadToStorage(byte[] data, string fileName, string mimeType, IDictionary<string, string> exifData = null)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentNullException(nameof(data));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            if (string.IsNullOrWhiteSpace(mimeType))
                throw new ArgumentNullException(nameof(mimeType));

            // Get the reference to the block blob from the container
            var blockBlob = _container.GetBlockBlobReference(fileName);

            // Upload the file
            blockBlob.Properties.ContentType = mimeType;

            await blockBlob.UploadFromByteArrayAsync(data, 0, data.Length);
            return blockBlob.Uri;
        }
    }
}
