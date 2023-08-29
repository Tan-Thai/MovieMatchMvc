using Microsoft.AspNetCore.Mvc;
using MovieMatchMvc.Models;

namespace MovieMatchMvc.Controllers
{
    public class MovieController : Controller
    {
		[HttpGet("/Watchlist")]
		public IActionResult Watchlist()
		{
			var model = MovieService.GetWatchlist();
			return View(model);
		}
	}
}
