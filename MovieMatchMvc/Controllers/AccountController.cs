using Microsoft.AspNetCore.Mvc;

namespace MovieMatchMvc.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
