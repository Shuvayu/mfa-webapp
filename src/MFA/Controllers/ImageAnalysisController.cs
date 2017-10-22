﻿using MFA.IService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MFA.WebApp.Controllers
{
    [Produces("application/json")]
    public class ImageAnalysisController : Controller
    {
        private readonly IImageAnalysisService _imageRecognitionService;

        public ImageAnalysisController(IImageAnalysisService imageRecognitionService)
        {
            _imageRecognitionService = imageRecognitionService;
        }

        [HttpGet]
        [Route("api/imageAnalysis/emotionalAnalysis")]
        public async Task<IActionResult> EmotionalAnalysis(Uri uri)
        {
            if (uri == null)
            {
                return BadRequest("Uri parameter is empty.");
            }
            try
            {
                string analysisResult = await _imageRecognitionService.GetEmotionalAnalysis(uri);
                return Ok(analysisResult);
            }
            catch (Exception e)
            {
                return BadRequest(string.Format("Error: {0}", e));
            }            
        }
    }
}