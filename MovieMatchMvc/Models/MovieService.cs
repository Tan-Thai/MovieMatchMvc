using MovieMatchMvc.Views.Movie;
using static System.Reflection.Metadata.BlobBuilder;

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieMatchMvc.Models
{
    public class MovieService
    {
        public async Task<List<SearchList>> FetchMovies(string query)
        {
            List<SearchList> movies = new List<SearchList>();

            using (HttpClient httpClient = new HttpClient())
            {
                string apiKey = "9484edbd5be7b021216db9b56a4f92b0";
                string apiUrl = $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={query}";

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(content);
                    JArray items = (JArray)jsonResponse["results"];

                    movies = items.Take(100).Select(i => new SearchList
                    {
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
