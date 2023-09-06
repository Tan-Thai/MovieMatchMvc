namespace MovieMatchMvc.Models
{
    public class WatchList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
        public string? Url { get; set;}
        public AccountUser? AccountUser { get; set; }
        public string? UserId { get; set; }
        public int MovieId { get; set; }
        public double? Popularity { get; set; }
		public DateTime? ReleaseDate { get; set; }
        public List<MovieGenres> Genres { get; set; }
	}
}
