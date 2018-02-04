using MFA.Entities.LogicModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MFA.IService
{
    public interface IEmotionAnalysisService
    {
        /// <summary>
        /// Gets the emotional analysis of the blob uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<List<EmotionResponse>> GetEmotionalAnalysisAsync(Uri uri);
    }
}
