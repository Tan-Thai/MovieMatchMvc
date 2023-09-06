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
		TMDbClient client = new TMDbClient("9484edbd5be7b021216db9b56a4f92b0");
		ApplicationContext context;

		public MovieService(ApplicationContext context)
		{
			this.context = context;
		}

		//Search actions - main page and search.
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
							MovieId = m.Id,
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
		public async Task<List<SearchVM>> SearchMoviesAsync(string query, string userId, int pageNumber)
		{
			List<SearchVM> movieBag = new List<SearchVM>();
			var myWatchlist = GetWatchlist(userId);
			var watchListHash = new HashSet<int>(myWatchlist.Select(w => w.MovieId));
			int resultsPerPage = 20;

			using (client)
			{
				SearchContainer<SearchMovie> initialSearchResults = await client.SearchMovieAsync(query, "en-US", pageNumber, false);
				int totalPages = initialSearchResults.TotalPages;

				var searchTasks = new List<Task<SearchContainer<SearchMovie>>>();

				for (int i = 1; i <= totalPages; i++)
				{
					int currentPage = i;
					searchTasks.Add(client.SearchMovieAsync(query, "en-US", currentPage, false));
				}
				var searchResults = await Task.WhenAll(searchTasks);

				var processingTasks = searchResults.Select(async searchResult =>
				{
					foreach (SearchMovie m in searchResult.Results)
					{
						SearchVM movie = CreateSearchVMBySearch(m, myWatchlist);
						if (watchListHash.Contains(movie.Id))
							movie.InWatchList = true;
						movieBag.Add(movie);
					}
				});
				await Task.WhenAll(processingTasks);
			}

			List<SearchVM> movieResult = movieBag
				.OrderByDescending(m => m.Popularity)
				.Skip((pageNumber - 1) * resultsPerPage)
				.Take(resultsPerPage)
				.ToList();

			return movieResult;
		}

		//Grabbing watchlist and userID
		public WatchlistVM[] GetWatchlist(string userId)
		{
			//.OrderBy(p => p.Title) //make this into switch statement and make it based on what input (might brick)
			return context.watchLists
				.Where(w => w.UserId == userId)
				.OrderBy(p => p.Title)
				.Select(p => new WatchlistVM //add more props to enable similar showcase as SearchView
				{
					Title = p.Title,
					Poster = p.Poster,
					MovieId = p.MovieId,
					ReleaseDate = p.ReleaseDate,
					Popularity = p.Popularity,
				})
				.ToArray();

		}
		public WatchlistVM[] GetWatchlist(string userId, string? orderby)
		{
			//.OrderBy(p => p.Title) //make this into switch statement and make it based on what input (might brick)
			IQueryable<WatchlistVM> watchListQuery = context.watchLists
				.Where(w => w.UserId == userId)
				.Select(p => new WatchlistVM //add more props to enable similar showcase as SearchView
				{
					Title = p.Title,
					Poster = p.Poster,
					MovieId = p.MovieId,
					ReleaseDate = p.ReleaseDate,
					Popularity = p.Popularity,
				});

			switch (orderby) //orderby switch that would basically allow us to sort by date added/release year etc.
			{
				case null:
					watchListQuery = watchListQuery.OrderBy(p => p.Title);
					break;

				case "Popularity":
					watchListQuery = watchListQuery.OrderByDescending(p => p.Popularity);
					break;

				case "ReleaseDate":
					watchListQuery = watchListQuery.OrderByDescending(p => p.ReleaseDate);
					break;

			}


			return watchListQuery.ToArray();
		}
		public string GetUserIdByUsername(string username)
		{

			if (context.accountUsers.Any(u => u.UserName == username))
			{
				return context.accountUsers
					.Where(u => u.UserName == username)
					.Select(u => u.Id)
					.FirstOrDefault();
			}
			else
			{
				return null;
			}
		}

		//Adding and removing movies from watchlist.
		public async Task AddMovieToWatchlistByIdAsync(int movieId, string userId)
		{
			var movie = client.GetMovieAsync(movieId).Result;
			await AddMovieToWatchlistAsync(movie, userId);
		}
		public async Task AddMovieToWatchlistAsync(Movie movie, string userId)
		{
			context.watchLists.Add(new WatchList //Add Genres as prop to be able to order by on "GetWatchList"
			{
				MovieId = movie.Id,
				Title = movie.Title,
				UserId = userId,
				Poster = "https://image.tmdb.org/t/p/w500" + movie.PosterPath,
				ReleaseDate = movie.ReleaseDate,
				Popularity = movie.Popularity,
				
			});
			await context.SaveChangesAsync();
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

		//MatchWatchList -
		internal object GetMatchedMovies(string? currentUserId, string otherUserId)
		{
			var myWatchlist = GetWatchlist(currentUserId);
			var searchedWatchlist = GetWatchlist(otherUserId);
			var commonMovieIds = myWatchlist.Select(m => m.MovieId).Intersect(searchedWatchlist.Select(m => m.MovieId)).ToList();
			var commonMovies = myWatchlist.Where(m => commonMovieIds.Contains(m.MovieId)).ToList();
			return commonMovies;
		}

		//DetailsVM - Anything related to Details page.
		public DetailsVM GetMovieDetailsById(int movieId, string currentUserId)
		{
			var myWatchlist = GetWatchlist(currentUserId);

			using (client)
			{
				var movie = client.GetMovieAsync(movieId).Result;
				var movieCredits = client.GetMovieCreditsAsync(movieId).Result;
				if (movie != null)
				{
					var movieDetails = CreateDetailsVM(movie, movieCredits);
					foreach (var m in myWatchlist)
					{
						if (m.MovieId == movieDetails.Id)
						{
							movieDetails.InWatchList = true;
						}
					}
					return movieDetails;
				}
				else
					return null;
			}
		}
		private DetailsVM CreateDetailsVM(Movie movie, Credits credits)
		{
			return new DetailsVM
			{
				Id = movie.Id,
				Title = movie.Title,
				Poster = "https://image.tmdb.org/t/p/w500" + movie.PosterPath,
				ReleaseDate = movie.ReleaseDate,
				Rating = movie.VoteAverage,
				Description = movie.Overview,
				Cast = credits.Cast,
				Crew = credits.Crew,
				Runtime = movie.Runtime,
				Genre = movie.Genres,

				BackDropPoster = "https://image.tmdb.org/t/p/w1920_and_h800_multi_faces" + movie.BackdropPath
			};
		}

		//Misc - Create SearchVM methods used above
		private SearchVM CreateSearchVMBySearch(SearchMovie movie, IEnumerable<WatchlistVM> myWatchlist)
		{
			bool inWatchList = myWatchlist?.Any(m => m.MovieId == movie.Id) ?? false;

			return new SearchVM
			{
				InWatchList = inWatchList,
				Id = movie.Id,
				Title = movie.Title,
				Poster = "https://image.tmdb.org/t/p/w500" + movie.PosterPath,
				ReleaseDate = movie.ReleaseDate,
				Rating = movie.VoteAverage,
				Popularity = movie.Popularity,
				Description = movie.Overview,
			};
		}

	}
}

		//private SearchVM CreateSearchVMById(Movie movie)
		//{
		//	return new SearchVM
		//	{
		//		Id = movie.Id,
		//		Title = movie.Title,
		//		Poster = "https://image.tmdb.org/t/p/w500" + movie.PosterPath,
		//		ReleaseDate = movie.ReleaseDate,
		//		Rating = movie.VoteAverage,
		//		Description = movie.Overview,
		//		Popularity = movie.Popularity
		//	};
		//}
		//public async Task<SearchVM> FetchMovieByIdAsync(int movieId)
		//{
		//	using (client)
		//	{
		//		var movie = client.GetMovieAsync(movieId).Result;

		//		if (movie != null)
		//			return CreateSearchVMById(movie);
		//		else
		//			return null;
		//	}
		//}