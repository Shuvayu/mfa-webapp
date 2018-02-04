using MFA.Entities.Configurations;
using MFA.Entities.LogicModels;
using MFA.IInfrastructure;
using MFA.IService;
using Microsoft.Extensions.Options;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Threading.Tasks;

namespace MFA.Service
{
    public class FaceAnalysisService : IFaceAnalysisService
    {
        private readonly IOptions<AzureConfiguration> _azureSettings;
        private readonly FaceServiceClient _faceServiceClient;
        private readonly IWaitCall _waitCall;

        public FaceAnalysisService(IOptions<AzureConfiguration> azureSettings, IWaitCall waitCall)
        {
            _azureSettings = azureSettings;
            _waitCall = waitCall;
            _faceServiceClient = new FaceServiceClient(_azureSettings.Value.CognitiveServicesFaceApiHeaderKey);
        }

        #region Add Person To Person Group

        public async Task<Guid> AddPersonToPersonGroupAsync(string personGroupId, string personName, string imageDirectory)
        {
            try
            {
                return await AddPersonDetailsAndImagesToPersonGroupAsync(personGroupId, personName, imageDirectory);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<Guid> AddPersonDetailsAndImagesToPersonGroupAsync(string personGroupId, string personName, string imageDirectory)
        {
            await _waitCall.WaitCallLimitPerMinAsync();
            var personDetails = await _faceServiceClient.CreatePersonAsync(personGroupId, personName);

            var faceListStream = ImageExtensions.GetAllImagesInADirectory(imageDirectory);

            foreach (var imageStream in faceListStream)
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
            catch (Exception)
            {
                throw;
            }
        }

        private async Task CreateAPersonGroupLogicalContainerAsync(string personGroupId, string personGroupName)
        {
            _waitCall.AddCallTimeToQueue();
            await _faceServiceClient.CreatePersonGroupAsync(personGroupId, personGroupName);
        }
        #endregion

        public Task<string> GetFacialAnalysisAsync(Uri uri)
        {
            throw new NotImplementedException();
        }

        #region Train Person Group

        public async Task TrainPersonGroupAsync(string personGroupId)
        {
            try
            {
                await TrainFaceApiAsync(personGroupId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task TrainFaceApiAsync(string personGroupId)
        {
            TrainingStatus trainingStatus = null;
            while (true)
            {
                await _waitCall.WaitCallLimitPerMinAsync();
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
