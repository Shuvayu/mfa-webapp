using MFA.Entities.Confugurations;
using MFA.Entities.Constants;
using MFA.Entities.LogicModels;
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
    public class ImageAnalysisService : IImageAnalysisService
    {
        private readonly IOptions<AzureConfiguration> _azureSettings;

        public ImageAnalysisService(IOptions<AzureConfiguration> azureSettings)
        {
            _azureSettings = azureSettings;
        }

        public async Task<string> GetEmotionalAnalysis(Uri uri)
        {
            var client = new HttpClient();
            var data = JsonConvert.SerializeObject(new { url = uri.ToString() });
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(_azureSettings.Value.CognitiveServicesEmotionApiUrl),
                Method = HttpMethod.Post
            };

            request.Headers.Add("Ocp-Apim-Subscription-Key", _azureSettings.Value.CognitiveServicesEmotionApiHeaderKey);
            request.Content = new StringContent(data, Encoding.UTF8, MediaTypes.ApplicationJson);

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<EmotionResponse>>(result);
            return result;
        }

        public Task<string> GetFacialAnalysis(Uri uri)
        {
            throw new NotImplementedException();
        }
    }
}