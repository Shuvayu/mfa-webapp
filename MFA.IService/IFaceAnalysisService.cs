using System;
using System.Collections.Generic;
using System.IO;
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
        /// <param name="images"></param>
        /// <returns></returns>
        Task<Guid> AddPersonToPersonGroupAsync(string personGroupId, string personName, List<Stream> images);

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
        /// <param name="personGroupId"></param>
        /// <returns></returns>
        Task<string> GetFacialAnalysisAsync(Uri uri, string personGroupId);

        /// <summary>
        /// Trains the Face Api to recognise the person Group
        /// </summary>
        /// <param name="personGroupId"></param>
        /// <returns></returns>
        Task TrainPersonGroupAsync(string personGroupId);
    }
}
