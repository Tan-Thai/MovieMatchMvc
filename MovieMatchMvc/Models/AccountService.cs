using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieMatchMvc.Views.Account;
using MovieMatchMvc.Views.Movie;

namespace MovieMatchMvc.Models
{
    public class AccountService
    {
        UserManager<AccountUser> userManager;
        SignInManager<AccountUser> signInManager;
        RoleManager<IdentityRole> roleManager;
        ApplicationContext context;
        MovieService _movieService;

        public AccountService(
            UserManager<AccountUser> userManager,
            SignInManager<AccountUser> signInManager,
            RoleManager<IdentityRole> roleManager,
             ApplicationContext context,
             MovieService movieService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.context = context;
            this._movieService = movieService;

        }

        public async Task<string[]?> TryRegisterAsync(RegisterVM viewModel)
        {
            var user = new AccountUser
            {
                UserName = viewModel.Username,
            };

            IdentityResult result = await userManager.CreateAsync(user, viewModel.Password);

            if (result.Succeeded)
                return null;
            else
                return result.Errors
                    .Select(o => o.Description)
                    .ToArray();
        }

        public async Task<string?> TryLoginAsync(LoginVM viewModel)
        {
            SignInResult result = await signInManager.PasswordSignInAsync(
                viewModel.Username,
                viewModel.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            return result.Succeeded ? null : "Login failed";
        }
        public async Task TryLogoutAsync()
        {
            await signInManager.SignOutAsync();
        }


        List<WatchList> movies = new List<WatchList>()
        {
            new WatchList { Id = 1, Title = "Star Wars", Poster = "https://image.tmdb.org/t/p/w500/gq5Wi7i4SF3lo4HHkJasDV95xI9.jpg" , Url = "temp"}
        };

        public WatchlistVM[] GetWatchlist(string userId)
        {
            return context.watchLists.Where(w => w.UserId == userId)
                .OrderBy(p => p.Title)
                .Select(p => new WatchlistVM { Title = p.Title, Poster = p.Poster })
                .ToArray();
        }

        public async Task AddToListAsync(SearchVM movie, string userId)
        {
            {
                context.watchLists.Add(
                    new WatchList
                    {
                        Title = movie.Title,
                        Poster = movie.Poster,
                        UserId = userId  // set current user ID
                    }
                );
                await context.SaveChangesAsync();
            }


        }
		public async Task AddMovieToWatchlistById(int movieId, string userId)
		{
			var movie = await _movieService.FetchMovieById(movieId);
			await AddMovieToWatchlist(movie, userId);
		}
		private async Task AddMovieToWatchlist(SearchVM movie, string userId)
		{
			context.watchLists.Add(new WatchList
			{
				Title = movie.Title,
				Poster = movie.Poster,
				UserId = userId
			});
			await context.SaveChangesAsync();
		}
	}
}
