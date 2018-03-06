using MFA.Entities.Configurations;
using MFA.Entities.LogicModels;
using MFA.Entities.ViewModels;
using MFA.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace MFA.WebApp.Controllers
{
    public class FaceAnalysisController : Controller
    {
        private readonly IFaceAnalysisService _faceAnalysisService;
        private readonly IOptions<AzureConfiguration> _azureSettings;

        public FaceAnalysisController(IOptions<AzureConfiguration> azureSettings, IFaceAnalysisService faceAnalysisService)
        {
            _azureSettings = azureSettings;
            _faceAnalysisService = faceAnalysisService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAsync(FaceTrainingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var imageList = ImageExtensions.GetAllUploadedImages(model.Files);
                await _faceAnalysisService.CreateAPersonGroupAsync(_azureSettings.Value.CognitiveServicesFaceApiGroupId, model.FaceName);
                await _faceAnalysisService.AddPersonToPersonGroupAsync(_azureSettings.Value.CognitiveServicesFaceApiGroupId, model.FaceName, imageList);
                await _faceAnalysisService.TrainPersonGroupAsync(_azureSettings.Value.CognitiveServicesFaceApiGroupId);
            }
            else
            {
                return RedirectToActionPermanent(nameof(Index), "FaceAnalysis");
            }

            return RedirectToActionPermanent(nameof(Index), "Home");
        }

        [HttpGet]
        [Route("api/facialAnalysisAsync")]
        public async Task<IActionResult> FacialAnalysisAsync(Uri uri)
        {
            if (uri == null)
            {
                return BadRequest("Uri parameter is empty.");
            }
            try
            {
                var analysisResult = await _faceAnalysisService.GetFacialAnalysisAsync(uri, _azureSettings.Value.CognitiveServicesFaceApiGroupId);
                return Ok(analysisResult);
            }
            catch (Exception e)
            {
                return BadRequest(string.Format("Error: {0}", e));
            }
        }
    }
}