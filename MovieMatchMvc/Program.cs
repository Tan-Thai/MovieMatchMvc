using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieMatchMvc.Models;

namespace MovieMatchMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            //builder.Services.AddTransient<AccountService>();

            // Hämta connection-strängen från AppSettings.json
            var connString = builder.Configuration.GetConnectionString("DefaultConnection");

            //// Registrera Context-klassen för dependency injection
            builder.Services.AddDbContext<ApplicationContext>(o => o.UseSqlServer(connString));

            //// Registera identity-klasserna och vilken DbContext som ska användas
            builder.Services.AddIdentity<AccountUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            // Specificera att cookies ska användas och URL till inloggnings-sidan
            builder.Services.ConfigureApplicationCookie(o => o.LoginPath = "/login");

            var app = builder.Build();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(o => o.MapControllers());

            app.Run();
        }
    }
}