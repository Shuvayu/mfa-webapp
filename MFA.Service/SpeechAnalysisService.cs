using MFA.Entities.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MFA.Service
{
    public class SpeechAnalysisService
    {
        private readonly IOptions<AzureConfiguration> _azureSettings;
        //private string _accessToken = string.Empty;
        //Access token expires every 10 minutes. Renew it every 9 minutes only.
        private const int _refreshTokenDuration = 9;
        private readonly ILogger _logger;


        public SpeechAnalysisService(IOptions<AzureConfiguration> azureSettings, ILoggerFactory logger)
        {
            _azureSettings = azureSettings;
            _logger = logger.CreateLogger(nameof(SpeechAnalysisService));
        }
    }
}
