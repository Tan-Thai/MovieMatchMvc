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
		public async Task<IActionResult> Search(string query)
		{
			if (string.IsNullOrEmpty(query))
			{
				return View("Index");
			}
			string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			List<SearchVM> movies = await _movieService.FetchMovies(query, currentUserId);
			ViewBag.Query = query;
			return View("Search", movies);
		}


		[HttpGet("/Watchlist")]
		public IActionResult Watchlist()
		{
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var model = _movieService.GetWatchlist(userId);
			return View("Watchlist", model);
		}
		[HttpPost]
		[Route("ManageWatchList")]
		public async Task<IActionResult> ManageWatchList(int movieId, bool remove = true, bool isJavaCall = false)
		{
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (remove)
				await _movieService.RemoveFromWatchListAsync(movieId, userId);
			else
				await _movieService.AddMovieToWatchlistById(movieId, userId);

			if (isJavaCall) //look to send a parameter rather than request.
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
					return View("Error");
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
			return RedirectToAction("MatchWatchLists", new { username });
		}

		[HttpGet("/Details/{id}")] // fix get id to work with multiple views, passing movie id properly
		public async Task<IActionResult> DetailsAsync(int id)
		{
			var movie = await _movieService.GetMovieById(id);
			return View(movie);
		}
	}
}

//[HttpPost]
//[Route("AddMovieToList")]
//public async Task<IActionResult> AddMovieToList(int movieId)
//{
//	string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//	await _movieService.AddMovieToWatchlistById(movieId, userId);
//	return Json(new { success = true });
//}
//------------
//[HttpPost]
//[Route("RemoveFromWatchListSearch")]
//public async Task<IActionResult> RemoveFromWatchListSearch(int movieId)
//{
//	Console.WriteLine($"Received Movie ID: {movieId} to be removed");
//	string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//	await _movieService.RemoveFromWatchListAsync(movieId, userId);
//	return Json(new { success = true });
//}
//------
//[HttpPost]
//[Route("RemoveFromWatchList")]
//public async Task<IActionResult> RemoveFromWatchList(int movieId)
//{
//	Console.WriteLine($"Received Movie ID: {movieId} to be removed");
//	string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//	await _movieService.RemoveFromWatchListAsync(movieId, userId);
//	return RedirectToAction(nameof(Watchlist));
//}
