﻿namespace MovieMatchMvc.Models
{
    public class WatchList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
        public string? Url { get; set;}
        public AccountUser? AccountUser { get; set; }
    }
}
