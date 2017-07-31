using System;
using System.Collections.Generic;
using System.Text;

namespace MFA.Entities.Confugurations
{
    public class AzureConfiguration
    {
        public string BlobBaseUrl { get; set; }
        public string BlobAccountName { get; set; }
        public string BlobAccessKey { get; set; }
        public string BlobContainerName { get; set; }
        public string CognitiveServicesEmotionApiUrl { get; set; }
        public string CognitiveServicesEmotionApiHeaderKey { get; set; }
        public string CognitiveServicesFaceApiUrl { get; set; }
    }
}
