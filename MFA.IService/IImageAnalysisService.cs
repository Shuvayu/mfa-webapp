using System;
using System.Threading.Tasks;

namespace MFA.IService
{
    public interface IImageAnalysisService
    {
        /// <summary>
        /// Gets the emotional analysis of the blob uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<string> GetEmotionalAnalysis(Uri uri);

        /// <summary>
        /// Gets the facial analysis of the blob uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<string> GetFacialAnalysis(Uri uri);
    }
}
