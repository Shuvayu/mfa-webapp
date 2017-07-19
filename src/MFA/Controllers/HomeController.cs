using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MFA.IService;
using Microsoft.AspNetCore.Http;

namespace MFA.Controllers
{
    public class HomeController : Controller
    {
        IImageStorageService _imageStore;

        public HomeController(IImageStorageService imageStore)
        {
            _imageStore = imageStore;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "Upload Image";

            return View();
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(IFormFile file)
        {
            if (file != null)
            {
                var image = file.OpenReadStream();
                string imageId = await _imageStore.StoreImage(image);
                return RedirectToAction("Display", new { imageId = imageId});
            }

            return View();
        }

        public IActionResult Display(string imageId)
        {
            ViewData["Message"] = "Uploaded Image";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
