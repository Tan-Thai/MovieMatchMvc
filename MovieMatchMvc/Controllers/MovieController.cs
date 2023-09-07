using Microsoft.AspNetCore.Mvc;
using MovieMatchMvc.Models;
using MovieMatchMvc.Views.Movie;
using System.Security.Claims;

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
		public async Task<IActionResult> Search(string query, int pageNumber = 1)
		{
			if (string.IsNullOrEmpty(query))
			{
				return View("Index");
			}
			string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			List<SearchVM> movies = await _movieService.SearchMoviesAsync(query, currentUserId, pageNumber);
			ViewBag.Query = query;
			ViewBag.PageNumber = pageNumber;
			return View("Search", movies);
		}


		[HttpGet("/Watchlist")]
		public IActionResult Watchlist()
		{
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			string orderby = Request.Query["argument"];
			string genre = Request.Query["genre"];
			var model = _movieService.GetWatchlist(userId, orderby, genre);
			return View("Watchlist", model);
		}
		[HttpPost]
		[Route("ManageWatchList")]
		public async Task<IActionResult> ManageWatchList(int movieId, bool remove = true, bool isJsonCall = false)
		{
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (remove)
				await _movieService.RemoveFromWatchListAsync(movieId, userId);
			else
				await _movieService.AddMovieToWatchlistByIdAsync(movieId, userId);

			if (isJsonCall) //look to send a parameter rather than request.
			{
				return Json(new { success = true });
			}
			else
			{
				return RedirectToAction(nameof(Watchlist));
			}
		}
		[HttpGet("MatchWatchLists")]
		public IActionResult MatchWatchLists(string username)
		{

			if (!string.IsNullOrEmpty(username))
			{
				string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				string otherUserId = _movieService.GetUserIdByUsername(username);
				if (string.IsNullOrEmpty(otherUserId))
                {
                    TempData["IsFormSubmitted"] = true;
                    ViewBag.OtherUsername = null;
					return View("MatchWatchLists");
				}
				var commonMovies = _movieService.GetMatchedMovies(currentUserId, otherUserId);
				ViewBag.OtherUsername = username;

				return View("MatchWatchLists", commonMovies);
			}
			return View();
		}
		[HttpPost("MatchWatchLists")]
		public IActionResult MatchWatchListsPost(string username)
		{
			TempData["LastSearchedUsername"] = username;
			if (username == null)
			{
				Console.WriteLine("username is null");
                return RedirectToAction("MatchWatchLists");
            }

			return RedirectToAction("MatchWatchLists", new { username });
		}

		[HttpGet("Details/{id}")] // fix get id to work with multiple views, passing movie id properly
		public IActionResult Details(int id)
		{
			string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var movie =  _movieService.GetMovieDetailsById(id, currentUserId);
			return View(movie);
		}
	}
}

