using System.Collections.Generic;
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



	}
}
