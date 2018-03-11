namespace MFA.Entities.Configurations
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
        public string CognitiveServicesFaceApiHeaderKey { get; set; }
        public string CognitiveServicesFaceApiGroupId { get; set; }

        public string CognitiveServicesSpeechApiUrl { get; set; }
        public string CognitiveServicesSpeechAuthApiUrl { get; set; }
        public string CognitiveServicesSpeechApiHeaderKey { get; set; }

        public int CognitiveServicesApiCallsPerMin { get; set; }
    }
}
