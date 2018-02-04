using System;
using System.IO;
using System.Threading.Tasks;

namespace MFA.IService
{
    public interface IImageStorageService
    {
        /// <summary>
        /// Stores the image in Azure blob storage
        /// </summary>
        /// <param name="image"></param>
        /// <returns>Image id</returns>
        Task<string> StoreImageAsync(Stream image);

        /// <summary>
        /// Returns and build the Azure image link
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        Uri ImageLink(string imageId);
    }
}
