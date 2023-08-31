using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieMatchMvc.Models;
using MovieMatchMvc.Views.Account;
using MovieMatchMvc.Views.Movie;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MovieMatchMvc.Controllers
{
    public class AccountController : Controller
    {
        AccountService accountService;
        public AccountController(AccountService accountService)
        {
            this.accountService = accountService;
        }

		[HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterVM viewModel)
        {
            if(!ModelState.IsValid)
                return View();

            var errors = await accountService.TryRegisterAsync(viewModel);
            if(errors?.Length > 0) 
            {
                foreach(var error in errors) 
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View();
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginVM viewModel)
        {
            if (!ModelState.IsValid)
                return View();

            var errorMessage = await accountService.TryLoginAsync(viewModel);
            if (errorMessage != null)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return View();
            }

            return RedirectToAction("Index", "Movie");
        }

        [HttpGet("/logout")]
        public async Task<IActionResult> LogOutAsync()
        {
            await accountService.TryLogoutAsync();
            return RedirectToAction(nameof(Login));
        }

		//hårdkodad lista, tar just nu inte ut användarens lista. TODO
		//[HttpGet("/Watchlist")]
		//public IActionResult Watchlist()
		//{
		//	string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the user ID
		//	var model = accountService.GetWatchlist(userId);
		//	return View("Watchlist", model);
		//}

	
		//[HttpPost]
		//[Route("Account/AddMovieToList")]
		//public async Task<IActionResult> AddMovieToList(int movieId)
		//{
		//	// Add the movie to the watchlist
		//	string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		//	await accountService.AddMovieToWatchlistById(movieId, userId);
		//	return RedirectToAction("Watchlist");
		//}


	}
}
