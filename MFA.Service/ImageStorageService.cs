using MFA.Entities.Configurations;
using MFA.IService;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MFA.Service
{
    public class ImageStorageService : IImageStorageService
    {
        private readonly IOptions<AzureConfiguration> _azureSettings;
        private readonly Uri _baseBlobUri;
        private readonly CloudBlobClient _blobClient;
        private readonly CloudBlobContainer _container;

        public ImageStorageService(IOptions<AzureConfiguration> azureSettings)
        {
            _azureSettings = azureSettings;
            _baseBlobUri = new Uri(_azureSettings.Value.BlobBaseUrl);
            _blobClient = new CloudBlobClient(_baseBlobUri, new StorageCredentials(_azureSettings.Value.BlobAccountName, _azureSettings.Value.BlobAccessKey));
            _container = _blobClient.GetContainerReference(_azureSettings.Value.BlobContainerName);
        }

        public Uri ImageLink(string imageId)
        {
            var sasPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.Now.AddMinutes(-1450),
                SharedAccessExpiryTime = DateTime.Now.AddMinutes(30)
            };
            var blob = _container.GetBlockBlobReference(imageId);
            var sasToken = blob.GetSharedAccessSignature(sasPolicy);
            return new Uri(_baseBlobUri, $"/{_azureSettings.Value.BlobContainerName}/{imageId}{sasToken}");
        }

        public async Task<string> StoreImageAsync(Stream image)
        {
            var NewBlobName = Guid.NewGuid().ToString();
            var blob = _container.GetBlockBlobReference(NewBlobName);
            await blob.UploadFromStreamAsync(image);
            return NewBlobName;
        }
    }
}