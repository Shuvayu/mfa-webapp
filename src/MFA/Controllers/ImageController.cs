using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MFA.Controllers
{
    public class ImageController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewData["Message"] = "Upload Image";

            return View();
        }

        public IActionResult Imageupload(IFormFile image)
        {
            ViewData["Message"] = "Upload Image Uploaded";

            return View();
        }
    }
}
