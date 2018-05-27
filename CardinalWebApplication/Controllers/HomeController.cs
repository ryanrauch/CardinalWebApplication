using CardinalWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CardinalWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Cardinal is still in the product development phase.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Cardinal Software, LLC";
            return View();
        }

        public IActionResult Policies()
        {
            ViewData["Message"] = "Privacy Policy";
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
