using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MovieMatchMvc.Models
{
    public class ApplicationContext :
        IdentityDbContext<AccountUser, IdentityRole, string>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<AccountUser> accountUsers { get; set; }
        public DbSet<WatchList> watchLists { get; set; }

    }

    
}
