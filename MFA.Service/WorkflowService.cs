using MFA.Entities.Configurations;
using MFA.Entities.Constants;
using MFA.Entities.LogicModels;
using MFA.IService;
using Microsoft.Extensions.Options;

namespace MFA.Service
{
    public class WorkFlowService : IWorkFlowService
    {
        private readonly IOptions<AppConfiguration> _appSettings;

        public WorkFlowService(IOptions<AppConfiguration> appSettings)
        {
            _appSettings = appSettings;
        }
        public string InitiateWorkflow(string faceName)
        {
            return faceName != ApplicationConstants.NoOneIdentified ? TextReponse.WelcomeText(_appSettings.Value.CompanyName, faceName, "John") : TextReponse.NonRecorgnizedWelcomeText(_appSettings.Value.CompanyName);
        }
    }
}
