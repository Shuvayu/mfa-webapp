using System;
using System.Threading.Tasks;

namespace MFA.IService
{
    public interface IFaceAnalysisService
    {
        /// <summary>
        /// Adds a person and their images to a person group and returns their assigned Id
        /// </summary>
        /// <param name="personGroupId"></param>
        /// <param name="personName"></param>
        /// <param name="ImageDirectory"></param>
        /// <returns></returns>
        Task<Guid> AddPersonToPersonGroupAsync(string personGroupId, string personName, string ImageDirectory);

        /// <summary>
        /// Create a person group
        /// </summary>
        /// <param name="personGroupId"></param>
        /// <param name="personGroupName"></param>
        /// <returns></returns>
        Task CreateAPersonGroupAsync(string personGroupId, string personGroupName);

        /// <summary>
        /// Gets the facial analysis of the blob uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<string> GetFacialAnalysisAsync(Uri uri);

        /// <summary>
        /// Trains the Face Api to recognise the person Group
        /// </summary>
        /// <param name="personGroupId"></param>
        /// <returns></returns>
        Task TrainPersonGroupAsync(string personGroupId);
    }
}
