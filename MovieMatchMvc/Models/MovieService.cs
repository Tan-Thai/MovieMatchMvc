using Microsoft.EntityFrameworkCore;
using MovieMatchMvc.Views.Movie;
using Newtonsoft.Json.Linq;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace MovieMatchMvc.Models
{
	public class MovieService
	{

		string ApiKey = "9484edbd5be7b021216db9b56a4f92b0";
		TMDbClient client = new TMDbClient("9484edbd5be7b021216db9b56a4f92b0");
		ApplicationContext context;

		public MovieService(ApplicationContext context)
		{
			this.context = context;
		}


		public async Task<List<IndexVM>> FetchTopMovies()
		{
			List<IndexVM> movieList = new List<IndexVM>();

			using (client)
			{

				SearchContainer<SearchMovie> searchResults = client.GetMoviePopularListAsync().Result;
				

				if (searchResults != null)
				{
					foreach (SearchMovie m in searchResults.Results.Take(6))
					{
						IndexVM movie = new IndexVM()
						{
							Title = m.Title,
							Description = m.Overview,
							ImageUrl = "https://image.tmdb.org/t/p/w500" + m.PosterPath
						};

						movieList.Add(movie);
					}
				}
			}

			return movieList;
		}

		public async Task<List<SearchVM>> FetchMovies(string query, string userId)
		{

			List<SearchVM> movieResults = new List<SearchVM>();
			var myWatchlist = GetWatchlist(userId);

			using (client)
			{
				SearchContainer<SearchMovie> searchResults = await client.SearchMovieAsync(query);

				foreach (SearchMovie m in searchResults.Results.Take(100))
				{
					SearchVM movie = new SearchVM()
					{
						InWatchList = false,
						Id = m.Id,
						Title = m.Title,
						Poster = "https://image.tmdb.org/t/p/w500" + m.PosterPath,
						ReleaseDate = m.ReleaseDate,
						Rating = m.VoteAverage,
						Description = m.Overview
					};

					movieResults.Add(movie);
				}
			}

			foreach (var m in myWatchlist)
			{
				foreach (var x in movieResults)
				{
					if (m.MovieId == x.Id)
					{
						Console.WriteLine(x.Title);
						x.InWatchList = true;
					}
				}
			}

			return movieResults;
		}

		public async Task<SearchVM> FetchMovieById(int movieId)
		{
			using (client)
			{

				Movie movie = client.GetMovieAsync(movieId).Result;

				if (movie != null)
				{
					return new SearchVM
					{
						Id = movie.Id,
						Title = movie.Title,
						Poster = "https://image.tmdb.org/t/p/w500" + movie.PosterPath,
						ReleaseDate = movie.ReleaseDate, 
						Rating = movie.VoteAverage		
					};
				}
				else
				{
					return null;
				}
			}
		}

		public WatchlistVM[] GetWatchlist(string userId)
		{
			return context.watchLists.Where(w => w.UserId == userId)
				.OrderBy(p => p.Title)
				.Select(p => new WatchlistVM { Title = p.Title, Poster = p.Poster, MovieId = p.MovieId })
				.ToArray();
		}

		public async Task AddMovieToWatchlistById(int movieId, string userId)
		{
			var movie = await FetchMovieById(movieId);
			await AddMovieToWatchlist(movie, userId);
		}

		public async Task AddMovieToWatchlist(SearchVM movie, string userId)
		{
			context.watchLists.Add(new WatchList //potentially add more props to fill out watchlist
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


		public async Task RemoveFromWatchListAsync(int movieId, string userId)
		{
			var moveToBeRemoved = await context.watchLists
				.SingleOrDefaultAsync(m => m.UserId == userId && m.MovieId == movieId);

			if (moveToBeRemoved != null)
			{
				context.Remove(moveToBeRemoved);
				await context.SaveChangesAsync();
			}
		}

        public DetailsVM? GetById(int movieId)
        {
            return context.watchLists
                .Where(m => m.Id == movieId)
                .Select(m => new DetailsVM
                {
                    Title = m.Title,
                    Poster = m.Poster,
                    Url = m.Url
                })
                .SingleOrDefault();

        }

    }
}
