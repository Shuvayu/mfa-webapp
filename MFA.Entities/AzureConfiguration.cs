using System;
using System.Collections.Generic;
using System.Text;

namespace MFA.Entities
{
    public class AzureConfiguration
    {
        public string BlobBaseUrl { get; set; }
        public string BlobAccountName { get; set; }
        public string BlobAccessKey { get; set; }
        public string BlobContainerName { get; set; }
    }
}
