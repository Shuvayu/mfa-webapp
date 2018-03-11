using MFA.Entities.Configurations;
using MFA.IInfrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace MFA.Infrastructure
{
    public class HttpClientsFactory : IHttpClientsFactory
    {
        public static Dictionary<string, HttpClient> _HttpClients { get; set; }
        private readonly IOptions<AzureConfiguration> _azureSettings;

        public HttpClientsFactory(IOptions<AzureConfiguration> azureSettings)
        {
            _HttpClients = new Dictionary<string, HttpClient>();
            _azureSettings = azureSettings;
            Initialize();
        }

        private void Initialize()
        {
            SetUpClient(_azureSettings.Value.CognitiveServicesEmotionApiUrl);
            SetUpClient(_azureSettings.Value.CognitiveServicesSpeechAuthApiUrl);
            SetUpClient(_azureSettings.Value.CognitiveServicesSpeechApiUrl);
        }

        private static void SetUpClient(string url)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(url)
            };

            _HttpClients.Add(url, client);
        }

        public Dictionary<string, HttpClient> Clients()
        {
            return _HttpClients;
        }

        public HttpClient Client(string key)
        {
            return Clients()[key];
        }
    }
}
