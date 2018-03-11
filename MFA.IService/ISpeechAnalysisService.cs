using System.Threading.Tasks;

namespace MFA.IService
{
    public interface ISpeechAnalysisService
    {
        /// <summary>
        /// Gets the JWT Auth Token from for the Bing Speech Api
        /// </summary>
        Task GetAuthTokenAsync();

        /// <summary>
        /// Converts text to speech
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        Task ConvertTextToSpeechAsync(string text);
    }
}
