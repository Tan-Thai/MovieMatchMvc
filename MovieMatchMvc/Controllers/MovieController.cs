﻿using Microsoft.AspNetCore.Mvc;
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
		[Route("AddMovieToList")]
		public async Task<IActionResult> AddMovieToList(int movieId)
		{
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			await _movieService.AddMovieToWatchlistById(movieId, userId);
			return Json(new { success = true });
		}

		[HttpGet("MatchWatchLists")]
		public IActionResult MatchWatchLists()
		{
			return View();
		}

		[HttpPost("MatchWatchLists")]
		public IActionResult MatchWatchLists(string username)
		{

			string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			string otherUserId = _movieService.GetUserIdByUsername(username);
			if (string.IsNullOrEmpty(otherUserId))
			{

				return View("Error");
			}
			var myWatchlist = _movieService.GetWatchlist(currentUserId);
			var otherWatchlist = _movieService.GetWatchlist(otherUserId);
			var commonMovieIds = myWatchlist.Select(m => m.MovieId).Intersect(otherWatchlist.Select(m => m.MovieId)).ToList();
			var commonMovies = myWatchlist.Where(m => commonMovieIds.Contains(m.MovieId)).ToList();
			ViewBag.OtherUsername = username;

			return View("MatchWatchLists", commonMovies);
		}

		[HttpPost]
		[Route("RemoveFromWatchList")]
		public async Task<IActionResult> RemoveFromWatchList(int movieId)
		{
			Console.WriteLine($"Received Movie ID: {movieId} to be removed");
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			await _movieService.RemoveFromWatchListAsync(movieId, userId);
			return RedirectToAction(nameof(Watchlist));
		}
		[HttpPost]
		[Route("RemoveFromWatchListSearch")]
		public async Task<IActionResult> RemoveFromWatchListSearch(int movieId)
		{
			Console.WriteLine($"Received Movie ID: {movieId} to be removed");
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			await _movieService.RemoveFromWatchListAsync(movieId, userId);
			return Json(new { success = true });
		}

	}
}
