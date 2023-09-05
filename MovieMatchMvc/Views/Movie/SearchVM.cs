namespace MovieMatchMvc.Views.Movie
{
    public class SearchVM
    {

        //const int maxPageSize = 50;
        //public int? PageIndex { get; set; } = 1;
        //private int _pageSize = 20;
        //public int PageSize
        //{
        //	get
        //	{
        //		return _pageSize;
        //	}
        //	set
        //	{
        //		_pageSize = (value > maxPageSize) ? maxPageSize : value;
        //	}
        //}

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
		public double Popularity { get; set; }
	}
}
