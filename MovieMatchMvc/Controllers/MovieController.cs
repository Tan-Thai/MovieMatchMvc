using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieMatchMvc.Models;
using MovieMatchMvc.Views.Movie;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;

namespace MovieMatchMvc.Controllers
{
    public class MovieController : Controller
    {

		private readonly MovieService _movieService;

		public MovieController(MovieService movieService)
		{
			this._movieService = movieService;
		}

        
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

			List<SearchVM> movies = await _movieService.FetchMovies(query);
			return View("Search", movies);
		}

		[HttpPost("search")]
		public IActionResult TestButton()
		{
			TMDbClient client = new TMDbClient("9484edbd5be7b021216db9b56a4f92b0");
			Movie movie = client.GetMovieAsync(47964).Result;

			Console.WriteLine($"Movie name: {movie.Title}");
			return RedirectToAction(nameof(Search));
		}
		[HttpGet("/Watchlist")]
		public IActionResult Watchlist()
		{
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the user ID
			var model = _movieService.GetWatchlist(userId);
			return View("Watchlist", model);
		}


		[HttpPost]
		[Route("Account/AddMovieToList")]
		public async Task<IActionResult> AddMovieToList(int movieId)
		{
			// Add the movie to the watchlist
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			await _movieService.AddMovieToWatchlistById(movieId, userId);
			return RedirectToAction("Watchlist");
		}



	}
}
