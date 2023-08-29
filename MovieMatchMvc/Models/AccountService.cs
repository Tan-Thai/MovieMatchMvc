using MovieMatchMvc.Views.Movie;

namespace MovieMatchMvc.Models
{
    public class AccountService
    {
		List<WatchList> movies = new List<WatchList>();
		public WatchlistVM[] GetWatchlist()
		{
			return movies
				.OrderBy(p => p.Title)
				.Select(p => new WatchlistVM
				{
					Title = p.Title,
					Poster = p.Poster,

				})
				.ToArray();
		}
	}
}
