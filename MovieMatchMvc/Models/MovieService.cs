using MovieMatchMvc.Views.Movie;
using static System.Reflection.Metadata.BlobBuilder;

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Movies;

namespace MovieMatchMvc.Models
{
    public class MovieService
    {

		string ApiKey = "9484edbd5be7b021216db9b56a4f92b0";
		TMDbClient Client = new TMDbClient("9484edbd5be7b021216db9b56a4f92b0");
		ApplicationContext context;

		public MovieService(ApplicationContext context)
		{
			this.context = context;
	
		}


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

		public async Task<List<SearchVM>> FetchMovies(string query, string userId)
		{

			List<SearchVM> movies = new List<SearchVM>();
			var myWatchlist = GetWatchlist(userId);

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
						InWatchList = false,
						Id = (int)i["id"],
						Title = (string)i["title"],
						Poster = "https://image.tmdb.org/t/p/w500" + (string)i["poster_path"],
						ReleaseDate = (string)i["release_date"],
						Rating = (double)i["vote_average"]
					}).ToList();
				}
			}

			foreach (var movie in myWatchlist)
			{
				foreach (var x in movies)
				{
					if (movie.MovieId == x.Id)
					{
						Console.WriteLine(x.Title);
						x.InWatchList = true;
					}
				}
			}

			return movies;
		}

		public async Task<SearchVM> FetchMovieById(int movieId)
		{
			using (HttpClient httpClient = new HttpClient())
			{

				string apiUrl = $"https://api.themoviedb.org/3/movie/{movieId}?api_key={ApiKey}";
				HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
				
				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					JObject jsonResponse = JObject.Parse(content);

					return new SearchVM
					{
						Id = movieId,
						Title = (string)jsonResponse["title"],
						Poster = "https://image.tmdb.org/t/p/w500" + (string)jsonResponse["poster_path"],
						ReleaseDate = (string)jsonResponse["release_date"],
						Rating = (double)jsonResponse["vote_average"]
					};
				}
				else
				{
					// Handle error
					return null;
				}
			}
		}

		public WatchlistVM[] GetWatchlist(string userId)
		{
			return context.watchLists.Where(w => w.UserId == userId)
				.OrderBy(p => p.Title)
				.Select(p => new WatchlistVM { Title = p.Title, Poster = p.Poster, MovieId = p.MovieId})
				.ToArray();
		}

		public async Task AddToListAsync(SearchVM movie, string userId)
		{
			{
				context.watchLists.Add(
					new WatchList
					{
                        MovieId = movie.Id,
                        Title = movie.Title,
						Poster = movie.Poster,
						UserId = userId  // set current user ID
					}
				);
				await context.SaveChangesAsync();
			}


		}
		public async Task AddMovieToWatchlistById(int movieId, string userId)
		{
			var movie = await FetchMovieById(movieId);
			await AddMovieToWatchlist(movie, userId);
		}
		public async Task AddMovieToWatchlist(SearchVM movie, string userId)
		{
			context.watchLists.Add(new WatchList
			{
				MovieId = movie.Id,
				Title = movie.Title,
				Poster = movie.Poster,
				UserId = userId
			});
			await context.SaveChangesAsync();
		}

        public string GetUserIdByUsername(string username)
        {
            return context.accountUsers
                .Where(u => u.UserName == username)
                .Select(u => u.Id)
                .FirstOrDefault();
        }



    }
}
