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
    }
}
