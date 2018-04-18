using MFA.Entities.Configurations;
using MFA.Entities.Constants;
using MFA.IInfrastructure;
using MFA.IService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MFA.Service
{
    public class FaceAnalysisService : IFaceAnalysisService
    {
        private readonly IOptions<AzureConfiguration> _azureSettings;
        private readonly FaceServiceClient _faceServiceClient;
        private readonly ILogger _logger;
        private readonly IWaitCall _waitCall;

        public FaceAnalysisService(IOptions<AzureConfiguration> azureSettings, IWaitCall waitCall, ILoggerFactory logger)
        {
            _azureSettings = azureSettings;
            _waitCall = waitCall;
            _logger = logger.CreateLogger(nameof(FaceAnalysisService));
            _faceServiceClient = new FaceServiceClient(_azureSettings.Value.CognitiveServicesFaceApiHeaderKey, _azureSettings.Value.CognitiveServicesFaceApiUrl);
        }

        #region Add Person To Person Group

        public async Task<Guid> AddPersonToPersonGroupAsync(string personGroupId, string personName, List<Stream> images)
        {
            try
            {
                return await AddPersonDetailsAndImagesToPersonGroupAsync(personGroupId, personName, images);
            }
            catch (FaceAPIException e)
            {
                _logger.LogError($"Error => {e.ErrorCode} - {e.ErrorMessage}");
                throw;
            }
        }

        private async Task<Guid> AddPersonDetailsAndImagesToPersonGroupAsync(string personGroupId, string personName, List<Stream> images)
        {
            await _waitCall.WaitCallLimitPerMinAsync();
            var personDetails = await _faceServiceClient.CreatePersonAsync(personGroupId, personName);

            foreach (var imageStream in images)
            {
                await _waitCall.WaitCallLimitPerMinAsync();
                await _faceServiceClient.AddPersonFaceAsync(personGroupId, personDetails.PersonId, imageStream);
            }

            return personDetails.PersonId;
        }

        #endregion

        #region Create A Person Group

        public async Task CreateAPersonGroupAsync(string personGroupId, string personGroupName)
        {
            try
            {
                await CreateAPersonGroupLogicalContainerAsync(personGroupId, personGroupName);
            }
            catch (FaceAPIException e)
            {
                if (e.ErrorCode != "PersonGroupExists")
                {
                    _logger.LogError($"Error => {e.ErrorCode} - {e.ErrorMessage}");
                }
            }
        }

        private async Task CreateAPersonGroupLogicalContainerAsync(string personGroupId, string personGroupName)
        {
            _waitCall.AddCallTimeToQueue();
            await _faceServiceClient.CreatePersonGroupAsync(personGroupId, personGroupName);
        }
        #endregion

        #region Get Facial Analysis
        public async Task<string> GetFacialAnalysisAsync(Uri uri, string personGroupId)
        {
            try
            {
                return await IdentifyFacesAsync(uri, personGroupId);
            }
            catch (FaceAPIException e)
            {
                _logger.LogError($"Error => {e.ErrorCode} - {e.ErrorMessage}");

                throw;
            }
        }

        private async Task<string> IdentifyFacesAsync(Uri uri, string personGroupId)
        {
            _waitCall.AddCallTimeToQueue();
            var faces = await _faceServiceClient.DetectAsync(uri.ToString());
            var faceIds = faces.Select(face => face.FaceId).ToArray();

            _waitCall.AddCallTimeToQueue();
            var results = await _faceServiceClient.IdentifyAsync(personGroupId, faceIds);
            foreach (var identifyResult in results)
            {
                if (identifyResult.Candidates.Length == 0)
                {
                    return ApplicationConstants.NoOneIdentified;
                }
                else
                {
                    var candidateId = identifyResult.Candidates[0].PersonId;
                    _waitCall.AddCallTimeToQueue();
                    var person = await _faceServiceClient.GetPersonAsync(personGroupId, candidateId);
                    return $"{person.Name}";
                }
            }
            return string.Empty;
        }
        #endregion

        #region Train Person Group

        public async Task TrainPersonGroupAsync(string personGroupId)
        {
            try
            {
                await TrainFaceApiAsync(personGroupId);
            }
            catch (FaceAPIException e)
            {
                _logger.LogError($"Error => {e.ErrorCode} - {e.ErrorMessage}");

                throw;
            }
        }

        private async Task TrainFaceApiAsync(string personGroupId)
        {
            TrainingStatus trainingStatus = null;
            while (true)
            {
                await _waitCall.WaitCallLimitPerMinAsync();
                await _faceServiceClient.TrainPersonGroupAsync(personGroupId);
                trainingStatus = await _faceServiceClient.GetPersonGroupTrainingStatusAsync(personGroupId);

                if (trainingStatus.Status != Status.Running)
                {
                    break;
                }

                await Task.Delay(1000);
            }
        }
    }
    #endregion
}
