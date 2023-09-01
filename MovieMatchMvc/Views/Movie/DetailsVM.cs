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
	}
}
