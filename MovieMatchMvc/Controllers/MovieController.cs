using Microsoft.AspNetCore.Mvc;

namespace MovieMatchMvc.Controllers
{
    public class MovieController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
