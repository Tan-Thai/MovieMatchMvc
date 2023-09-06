using Newtonsoft.Json;

namespace MovieMatchMvc.Models
{
	public class MovieGenres
	{

		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
		public int TmdbId { get; set; }

	}
}
