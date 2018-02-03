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
            var client = new HttpClient
            {
                BaseAddress = new Uri(_azureSettings.Value.CognitiveServicesEmotionApiUrl)
            };
            _HttpClients.Add(nameof(_azureSettings.Value.CognitiveServicesEmotionApiUrl), client);

            client.BaseAddress = new Uri(_azureSettings.Value.CognitiveServicesFaceApiUrl);
            _HttpClients.Add(nameof(_azureSettings.Value.CognitiveServicesFaceApiUrl), client);
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
