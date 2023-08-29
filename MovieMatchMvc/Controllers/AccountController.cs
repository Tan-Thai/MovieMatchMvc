using Microsoft.AspNetCore.Mvc;
using MovieMatchMvc.Models;

namespace MovieMatchMvc.Controllers
{
    public class AccountController : Controller
    {
		[HttpGet("/Watchlist")]
		public IActionResult Watchlist(AccountService accountService)
		{
			var model = accountService.GetWatchlist();
			return View(model);
		}
	}
}
