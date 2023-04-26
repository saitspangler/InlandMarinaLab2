using Microsoft.AspNetCore.Mvc;

namespace InlandMarinaMVCApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Console.WriteLine(HttpContext.Session.GetString("CurrentCustomer"));
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
