﻿using MFA.IService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MFA.WebApp.Controllers
{
    [Produces("application/json")]
    public class EmotionAnalysisController : Controller
    {
        private readonly IEmotionAnalysisService _imageRecognitionService;

        public EmotionAnalysisController(IEmotionAnalysisService imageRecognitionService)
        {
            _imageRecognitionService = imageRecognitionService;
        }

        [HttpGet]
        [Route("api/emotionalAnalysisAsync")]
        public async Task<IActionResult> EmotionalAnalysisAsync(Uri uri)
        {
            if (uri == null)
            {
                return BadRequest("Uri parameter is empty.");
            }
            try
            {
                var analysisResult = await _imageRecognitionService.GetEmotionalAnalysisAsync(uri);
                return Ok(analysisResult.Select(x => x.EmotionAnalyzed));
            }
            catch (Exception e)
            {
                return BadRequest(string.Format("Error: {0}", e));
            }
        }
    }
}