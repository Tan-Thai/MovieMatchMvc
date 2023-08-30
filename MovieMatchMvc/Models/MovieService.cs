﻿using MovieMatchMvc.Views.Movie;
using static System.Reflection.Metadata.BlobBuilder;

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TMDbLib.Client;

namespace MovieMatchMvc.Models
{
    public class MovieService
    {

		string ApiKey = "9484edbd5be7b021216db9b56a4f92b0";
		TMDbClient Client = new TMDbClient("9484edbd5be7b021216db9b56a4f92b0");

		public async Task<List<IndexVM>> FetchTopMovies()
        {
            List<IndexVM> movies = new List<IndexVM>();

            using (HttpClient httpClient = new HttpClient())
            {

                string apiUrl = $"https://api.themoviedb.org/3/movie/popular?api_key={ApiKey}";

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(content);
                    JArray items = (JArray)jsonResponse["results"];

                    movies = items.Take(6).Select(i => new IndexVM
                    {
                        Title = (string)i["title"],
                        Description = (string)i["overview"],
                        ImageUrl = "https://image.tmdb.org/t/p/w500" + (string)i["poster_path"]
                    }).ToList();
                }
            }

            return movies;
        }

		public async Task<List<SearchVM>> FetchMovies(string query)
		{
			List<SearchVM> movies = new List<SearchVM>();

			using (HttpClient httpClient = new HttpClient())
			{
				string apiUrl = $"https://api.themoviedb.org/3/search/movie?api_key={ApiKey}&query={query}";

				HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					JObject jsonResponse = JObject.Parse(content);
					JArray items = (JArray)jsonResponse["results"];

					movies = items.Take(100).Select(i => new SearchVM
					{
						Id = (int)i["id"],
						Title = (string)i["title"],
						Poster = "https://image.tmdb.org/t/p/w500" + (string)i["poster_path"],
						ReleaseDate = (string)i["release_date"],
						Rating = (double)i["vote_average"]
					}).ToList();
				}
			}

			return movies;
		}

		

		public IndexVM[] GetWatchlist()
		{
            return null; //Temp
			//return movies
			//	.OrderBy(p => p.Name)
			//	.Select(p => new IndexVM
			//	{
			//		Title = p.Title,
			//		Poster = p.Poster,
					 
			//	})
			//	.ToArray();
		}
	}
}
