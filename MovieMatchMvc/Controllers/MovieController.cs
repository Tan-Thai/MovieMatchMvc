using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieMatchMvc.Models;
using MovieMatchMvc.Views.Movie;

namespace MovieMatchMvc.Controllers
{
    public class MovieController : Controller
    {
		[HttpGet("/Watchlist")]
		public IActionResult Watchlist()
		{
			//var model = MovieService.GetWatchlist();
			return View();
		}
	
        private readonly MovieService _movieService = new MovieService();

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<IndexVM> movies = await _movieService.FetchTopMovies();
            return View(movies);
        }


		[HttpGet("search")]
		public async Task<IActionResult> Search(string query)
		{
			if (string.IsNullOrEmpty(query))
			{
				return View("Index");
			}

			List<SearchList> movies = await _movieService.FetchMovies(query);
			return View("Search", movies);
		}

	}
}
