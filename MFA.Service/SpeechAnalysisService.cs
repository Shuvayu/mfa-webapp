using MFA.Entities.Configurations;
using MFA.Entities.Constants;
using MFA.Entities.Enums;
using MFA.IInfrastructure;
using MFA.IService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MFA.Service
{
    public class SpeechAnalysisService : ISpeechAnalysisService
    {
        private readonly IOptions<AzureConfiguration> _azureSettings;
        private readonly IOptions<AppConfiguration> _appSettings;
        private readonly IHttpClientsFactory _httpClient;
        private readonly CookieContainer _cookieContainer;
        private string _accessToken = string.Empty;
        private string _voiceName = "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)";
        private string _locale = "en-IN";
        private readonly ILogger _logger;


        public SpeechAnalysisService(IOptions<AzureConfiguration> azureSettings, IOptions<AppConfiguration> appSettings, IHttpClientsFactory httpClientsFactory, ILoggerFactory logger)
        {
            _azureSettings = azureSettings;
            _appSettings = appSettings;
            _httpClient = httpClientsFactory;
            _logger = logger.CreateLogger(nameof(SpeechAnalysisService));
            _cookieContainer = new CookieContainer();
        }

        #region Auth Token WorkFlow
        public async Task GetAuthTokenAsync()
        {
            try
            {
                await InitiateTokenWorkFlowAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error => Inner :{e.InnerException} => Message :{e.Message}");
                throw;
            }
        }

        private async Task InitiateTokenWorkFlowAsync()
        {
            var client = _httpClient.Client(_azureSettings.Value.CognitiveServicesSpeechAuthApiUrl);
            client.DefaultRequestHeaders.Add(AzureConstants.OcpApimSubscriptionKey, _azureSettings.Value.CognitiveServicesSpeechApiHeaderKey);
            var response = await client.PostAsync(_azureSettings.Value.CognitiveServicesSpeechAuthApiUrl, null);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ApplicationException(response.RequestMessage.ToString());
            }
            _accessToken = await response.Content.ReadAsStringAsync();
        }
        #endregion

        #region Text To Speech Conversion Calls
        public async Task<string> ConvertTextToSpeechAsync(string text)
        {
            try
            {
                return await InitiateCallForTextToSpeechConversionAsync(text);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error => Inner :{e.InnerException} => Message :{e.Message}");
                throw;
            }
        }

        private async Task<string> InitiateCallForTextToSpeechConversionAsync(string text)
        {
            using (var request = new HttpRequestMessage
            {
                RequestUri = new Uri(_azureSettings.Value.CognitiveServicesSpeechApiUrl),
                Method = HttpMethod.Post,
            })
            {
                request.Headers.Add(AzureConstants.OcpApimSubscriptionKey, _azureSettings.Value.CognitiveServicesSpeechApiHeaderKey);
                request.Headers.Add("Authorization", "Bearer " + _accessToken);
                request.Headers.Add("X-Search-AppId", Guid.NewGuid().ToString().Replace("-", ""));
                request.Headers.Add("X-Search-ClientID", Guid.NewGuid().ToString().Replace("-", ""));
                request.Headers.Add("User-Agent", _appSettings.Value.ApplicationName);
                request.Headers.Add("X-Microsoft-OutputFormat", "riff-16khz-16bit-mono-pcm");
                request.Content = new StringContent(GenerateSsml(_locale, Gender.Female.ToString(), _voiceName, text));
                var responseMessage = await _httpClient.Client(_azureSettings.Value.CognitiveServicesSpeechApiUrl).SendAsync(request);
                if (responseMessage != null && responseMessage.IsSuccessStatusCode)
                {

                    var httpStream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    var fileName = Guid.NewGuid();
                    using (var file = File.OpenWrite(Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/audio", $"{ fileName }.wav")))
                    {
                        httpStream.CopyTo(file);
                        file.Flush();
                    }
                    return fileName.ToString();
                }
                else
                {
                    _logger.LogError($"Error => Inner :{responseMessage.RequestMessage.ToString()}");
                    throw new ApplicationException(responseMessage.RequestMessage.ToString());
                }
            }
        }

        /// <summary>
        /// Generates SSML.
        /// </summary>
        /// <param name="locale">The locale.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="name">The voice name.</param>
        /// <param name="text">The text input.</param>
        private static string GenerateSsml(string locale, string gender, string name, string text)
        {
            var ssmlDoc = new XDocument(
                    new XElement("speak",
                  new XAttribute("version", "1.0"),
                  new XAttribute(XNamespace.Xml + "lang", "en-US"),
                  new XElement("voice",
                    new XAttribute(XNamespace.Xml + "lang", locale),
                    new XAttribute(XNamespace.Xml + nameof(gender), gender),
                    new XAttribute(nameof(name), name),
                      text)));
            return ssmlDoc.ToString();
        }
        #endregion
    }
}
