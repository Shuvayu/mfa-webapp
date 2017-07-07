using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MFA.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Recognising faces with the help of Microsoft Cognitive Services APIs";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Shuvayu Dhar";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
