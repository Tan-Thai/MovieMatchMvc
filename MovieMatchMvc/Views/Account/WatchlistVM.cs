﻿namespace MovieMatchMvc.Views.Account
{
    public class WatchlistVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Poster { get; set; }
        public double Rating { get; set; }
        public string? Url { get; set; }
    }
}
