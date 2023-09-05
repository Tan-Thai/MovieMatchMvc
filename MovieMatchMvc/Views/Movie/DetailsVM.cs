using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace MovieMatchMvc.Views.Movie
{
    public class DetailsVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
		public string Description { get; set; }
        public string Poster { get; set; }
		public double Rating { get; set; }
        public string? Url { get; set; }
		public DateTime? ReleaseDate { get; set; }
		public bool InWatchList { get; set; }
		public string BackDropPoster { get;  set; }
		public List<Cast> Cast { get; set; }

		public int? Runtime { get; set; }
		public List<Crew> Crew { get; internal set; }
		public List<Genre> Genre { get; internal set; }
	}
}
