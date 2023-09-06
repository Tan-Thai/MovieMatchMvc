namespace MovieMatchMvc.Views.Movie
{
    public class SearchVM
    {

        public int PageNumber { get; set; }
        public int TotalPageCount { get; set; }

        public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Poster { get; set; }
		public double Rating { get; set; }
		public string? Url { get; set; }
		public DateTime? ReleaseDate { get; set; }
		public bool InWatchList { get; set; }
		public double? Popularity { get; set; }
	}
}
