using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Trine.Mobile.Dal
{
    /// <summary>
    /// Connector to an Azure Blob Storage
    /// </summary>
    public interface IImageAttachmentStorageRepository
    {
        /// <summary>
        /// Uploads data to the storage
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="fileName">File Name</param>
        /// <param name="mimeType">Mime type of the file</param>
        /// <param name="exifData">Exif data linked to the file</param>
        /// <returns>An access URI</returns>
        Task<Uri> UploadToStorage(byte[] data, string fileName, string mimeType, IDictionary<string, string> exifData = null);
    }
}
