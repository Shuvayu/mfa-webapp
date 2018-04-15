using MFA.Entities.LogicModels;
using MFA.IService;

namespace MFA.Service
{
    public class WorkFlowService : IWorkFlowService
    {
        public string InitiateWorkflow(string faceName)
        {
            return faceName != "No one identified" ? TextReponse.WelcomeText(faceName, "John") : TextReponse.NonRecorgnizedWelcomeText();
        }
    }
}
