using Microsoft.AspNetCore.Identity;

namespace MovieMatchMvc.Models
{
    public class AccountUser : IdentityUser
    {
        public List<WatchList>? Movies {get;set;}

    }
}
