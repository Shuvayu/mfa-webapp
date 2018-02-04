using MFA.Entities.ViewModels;
using MFA.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MFA.Controllers
{
    public class HomeController : Controller
    {
        private readonly IImageStorageService _imageStore;

        public HomeController(IImageStorageService imageStore)
        {
            _imageStore = imageStore;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "Upload Image";

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadAsync(IFormFile file)
        {
            if (file != null)
            {
                var image = file.OpenReadStream();
                var imageId = await _imageStore.StoreImageAsync(image);
                return RedirectToAction(nameof(Display), new { imageId = imageId });
            }

            return View();
        }

        public IActionResult Display(string imageId)
        {
            ViewData["Message"] = "Uploaded Image";
            var actionModel = new DisplayViewModel
            {
                ImageUrl = _imageStore.ImageLink(imageId)
            };
            return View(actionModel);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
