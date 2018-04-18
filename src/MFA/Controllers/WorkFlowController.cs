using MFA.Entities.Configurations;
using MFA.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace MFA.WebApp.Controllers
{
    public class WorkFlowController : Controller
    {
        private readonly ICalendarService _calendarService;
        private readonly IFaceAnalysisService _faceAnalysisService;
        private readonly IWorkFlowService _workFlowService;
        private readonly IOptions<AzureConfiguration> _azureSettings;

        public WorkFlowController(ICalendarService calendarService, IFaceAnalysisService faceAnalysisService, IOptions<AzureConfiguration> azureSettings, IWorkFlowService workFlowService)
        {
            _calendarService = calendarService;
            _faceAnalysisService = faceAnalysisService;
            _azureSettings = azureSettings;
            _workFlowService = workFlowService;
        }
        public IActionResult Index()
        {
            var calendarEvents = _calendarService.GetCalendarEvents();
            return View();
        }

        [HttpGet]
        [Route("api/workFlowAsync")]
        public async Task<IActionResult> EmotionalAnalysisAsync(Uri uri)
        {
            if (uri == null)
            {
                return BadRequest("Uri parameter is empty.");
            }
            try
            {
                var analysisResult = await _faceAnalysisService.GetFacialAnalysisAsync(uri, _azureSettings.Value.CognitiveServicesFaceApiGroupId);
                var workFlowResult = _workFlowService.InitiateWorkflow(analysisResult);
                return Ok(workFlowResult);
            }
            catch (Exception e)
            {
                return BadRequest(string.Format("Error: {0}", e));
            }
        }
    }
}