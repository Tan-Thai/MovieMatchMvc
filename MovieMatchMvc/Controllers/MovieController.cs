using Microsoft.AspNetCore.Mvc;

namespace MovieMatchMvc.Controllers
{
    public class MovieController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
