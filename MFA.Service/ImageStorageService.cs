using MFA.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace MFA.Service
{
    public class ImageStorageService : IImageStorageService
    {
        public Uri ImageLink(string imageId)
        {
            throw new NotImplementedException();
        }

        public Task<string> StoreImage(Stream image)
        {
            throw new NotImplementedException();
        }
    }
}
