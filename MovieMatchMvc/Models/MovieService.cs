using MovieMatchMvc.Views.Movie;
using static System.Reflection.Metadata.BlobBuilder;

namespace MovieMatchMvc.Models
{
    public class MovieService
    {
		public IndexVM[] GetWatchlist()
		{
			return movies
				.OrderBy(p => p.Name)
				.Select(p => new IndexVM
				{
					Title = p.Title,
					Poster = p.Poster,
					 
				})
				.ToArray();
		}
	}
}
