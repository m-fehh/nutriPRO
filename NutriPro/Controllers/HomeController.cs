using NutriPro.Data.Models.Management;
using NutriProTech.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace NutriProTech.Controllers
{
    public class HomeController : Controller
    {
        private readonly NutriProSession _session;

        public HomeController(NutriProSession session)
        {
            _session = session;
        }

        public IActionResult Index()
        {
            ViewBag.UserName = _session.UserName?.ToString().ToUpper();
            return View();
        }


        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}