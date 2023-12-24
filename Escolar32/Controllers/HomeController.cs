using Microsoft.AspNetCore.Mvc;

namespace Escolar32.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
