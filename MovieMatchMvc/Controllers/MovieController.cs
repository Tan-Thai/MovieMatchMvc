using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieMatchMvc.Models;

namespace MovieMatchMvc.Controllers
{
    public class MovieController : Controller
    {
        private readonly MovieService _movieService = new MovieService();

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<WatchList> movies = await _movieService.FetchMovies("harry");
            return View(movies);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View("Index");
            }

            List<WatchList> movies = await _movieService.FetchMovies(query);
            return View("Index", movies);
        }

    }
}
