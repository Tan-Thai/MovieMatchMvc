﻿using Microsoft.EntityFrameworkCore;
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
		public async Task<List<SearchVM>> FetchMovies(string query, string userId, int pageNumber)
		{

			List<SearchVM> movieBag = new List<SearchVM>();
			var myWatchlist = GetWatchlist(userId);
			var watchListHash = new HashSet<int>(myWatchlist.Select(w => w.MovieId));
			int resultsPerPage = 20;

			using (client)
			{
				SearchContainer<SearchMovie> initialSearchResults = await client.SearchMovieAsync(query,"en", pageNumber, false);
				int totalPages = initialSearchResults.TotalPages;

				for (int i = 1; i <= totalPages; i++)
				{
					SearchContainer<SearchMovie> searchResults = await client.SearchMovieAsync(query, "en", i, false);

					foreach (SearchMovie m in searchResults.Results)
					{
						SearchVM movie = CreateSearchVM(m, myWatchlist);
						if (watchListHash.Contains(movie.Id))
							movie.InWatchList = true;
						movieBag.Add(movie);
					}
				}
			}

			List<SearchVM> movieResult = movieBag
				.OrderByDescending(m => m.Popularity)
				.Skip((pageNumber - 1) * resultsPerPage)
				.Take(resultsPerPage)
				.ToList();

			return movieResult;
		}
		public async Task<SearchVM> FetchMovieById(int movieId)
		{
			using (client)
			{
				var movie = client.GetMovieAsync(movieId).Result;

				if (movie != null)
					return CreateSearchVM(movie);
				else
					return null;
			}
		}

		public WatchlistVM[] GetWatchlist(string userId)
		{
			return context.watchLists
				.Where(w => w.UserId == userId)
				.OrderBy(p => p.Title)
				.Select(p => new WatchlistVM { Title = p.Title, Poster = p.Poster, MovieId = p.MovieId })
				.ToArray();
		}
		public string GetUserIdByUsername(string username)
		{
			return context.accountUsers
				.Where(u => u.UserName == username)
				.Select(u => u.Id)
				.FirstOrDefault();
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
		internal object GetMatchedMovies(string? currentUserId, string otherUserId)
		{
			var myWatchlist = GetWatchlist(currentUserId);
			var searchedWatchlist = GetWatchlist(otherUserId);
			var commonMovieIds = myWatchlist.Select(m => m.MovieId).Intersect(searchedWatchlist.Select(m => m.MovieId)).ToList();
			var commonMovies = myWatchlist.Where(m => commonMovieIds.Contains(m.MovieId)).ToList();
			return commonMovies;
		}

		public DetailsVM GetMovieById(int movieId, string currentUserId)
		{
			var myWatchlist = GetWatchlist(currentUserId);

			using (client)
			{
				var movie = client.GetMovieAsync(movieId).Result;

				if (movie != null)
				{
					var movieDetails = CreateDetailsVM(movie);
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
		private DetailsVM CreateDetailsVM(Movie movie)
		{
			return new DetailsVM
			{
				Id = movie.Id,
				Title = movie.Title,
				Poster = "https://image.tmdb.org/t/p/w500" + movie.PosterPath,
				ReleaseDate = movie.ReleaseDate,
				Rating = movie.VoteAverage,
				Description = movie.Overview,
				//Actors = movie.Credits.Cast,
				BackDropPoster = "https://image.tmdb.org/t/p/w1920_and_h800_multi_faces" + movie.BackdropPath
			};
		}
		private SearchVM CreateSearchVM(Movie movie)
		{
			return new SearchVM
			{
				Id = movie.Id,
				Title = movie.Title,
				Poster = "https://image.tmdb.org/t/p/w500" + movie.PosterPath,
				ReleaseDate = movie.ReleaseDate,
				Rating = movie.VoteAverage,
				Description = movie.Overview
			};
		}
		private SearchVM CreateSearchVM(SearchMovie movie, IEnumerable<WatchlistVM> myWatchlist)
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
