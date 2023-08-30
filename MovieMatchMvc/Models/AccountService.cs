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


	}
}
