﻿using MFA.Entities.Configurations;
using MFA.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace MFA.WebApp.Controllers
{
    public class SpeechAnalysisController : Controller
    {
        private readonly ISpeechAnalysisService _speechAnalysisService;
        private readonly IOptions<AzureConfiguration> _azureSettings;

        public SpeechAnalysisController(IOptions<AzureConfiguration> azureSettings, ISpeechAnalysisService speechAnalysisService)
        {
            _azureSettings = azureSettings;
            _speechAnalysisService = speechAnalysisService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAsync(string speechText)
        {
            await _speechAnalysisService.GetAuthTokenAsync();
            await _speechAnalysisService.ConvertTextToSpeechAsync(speechText);
            return Ok("It worked !!!");
        }
    }
}