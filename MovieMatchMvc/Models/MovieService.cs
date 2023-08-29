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
        public async Task<List<string>> FetchMovies(string query)
        {
            List<string> movies = new List<string>();

            using (HttpClient httpClient = new HttpClient())
            {
                string apiKey = "9484edbd5be7b021216db9b56a4f92b0"; // Replace with your API Key
                string apiUrl = $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={query}";

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(content);
                    JArray items = (JArray)jsonResponse["results"];

                    movies = items.Take(6).Select(i => (string)i["title"]).ToList();
                }
            }

            return movies;
        }
    }
		
	
}
