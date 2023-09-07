using MovieMatchMvc.Models;

namespace MovieMatchMvc.Views.Movie
{
    public class WatchlistVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Poster { get; set; }
        public double Rating { get; set; }
        public string? Url { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; }
        public double? Popularity { get; set; }
		public DateTime? ReleaseDate { get; set; }
		public List<MovieGenres> Genres { get; set; }
		public bool userExists { get; set; }
	}
}
