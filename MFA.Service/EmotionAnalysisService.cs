using MFA.Entities.Configurations;
using MFA.Entities.Constants;
using MFA.Entities.LogicModels;
using MFA.IInfrastructure;
using MFA.IService;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MFA.Service
{
    public class EmotionAnalysisService : IEmotionAnalysisService
    {
        private readonly IOptions<AzureConfiguration> _azureSettings;
        private readonly IHttpClientsFactory _httpClient;

        public EmotionAnalysisService(IOptions<AzureConfiguration> azureSettings, IHttpClientsFactory httpClientsFactory)
        {
            _azureSettings = azureSettings;
            _httpClient = httpClientsFactory;

        }

        #region GetEmotionalAnalysisAsync
        public async Task<List<EmotionResponse>> GetEmotionalAnalysisAsync(Uri uri)
        {
            try
            {
                return await GetImageAnalysisFromEmotionApiAsync(uri);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<EmotionResponse>> GetImageAnalysisFromEmotionApiAsync(Uri uri)
        {
            var data = JsonConvert.SerializeObject(new { url = uri.ToString() });
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(_azureSettings.Value.CognitiveServicesEmotionApiUrl),
                Method = HttpMethod.Post
            };

            request.Headers.Add(AzureConstants.OcpApimSubscriptionKey, _azureSettings.Value.CognitiveServicesEmotionApiHeaderKey);
            request.Content = new StringContent(data, Encoding.UTF8, MediaTypes.ApplicationJson);

            var response = await _httpClient.Client(_azureSettings.Value.CognitiveServicesEmotionApiUrl).SendAsync(request);
            var resultString = await response.Content.ReadAsStringAsync();
            var typedModelResult = JsonConvert.DeserializeObject<List<EmotionResponse>>(resultString);
            return typedModelResult;
        }
        #endregion
    }
}