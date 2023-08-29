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
            builder.Services.AddTransient<AccountService>();

            // H�mta connection-str�ngen fr�n AppSettings.json
            var connString = builder.Configuration.GetConnectionString("DefaultConnection");

            //// Registrera Context-klassen f�r dependency injection
            builder.Services.AddDbContext<ApplicationContext>(o => o.UseSqlServer(connString));

            //// Registera identity-klasserna och vilken DbContext som ska anv�ndas
            builder.Services.AddIdentity<AccountUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            // Specificera att cookies ska anv�ndas och URL till inloggnings-sidan
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