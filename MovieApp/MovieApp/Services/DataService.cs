using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace MovieApp.Services
{
    public static class DataService
    {
        private static TMDbClient client;
        public static TMDbConfig clientConfig;
        public static List<Genre> Genres;
        public static SearchContainer<SearchMovie> searchMoviesContainer;

        public static async Task Init()
        {
            client = new TMDbClient(MovieApp.Constants.Constants.APIKey);
            clientConfig = await client.GetConfigAsync();
           
        }

        public static async Task<ConfigImageTypes> GetImageConfig()
        {
            if (clientConfig == null)
               await Init();
            return clientConfig.Images;
        }

        public static async Task<List<Genre>> GetGenresListAsync()
        {
            Genres = await client.GetMovieGenresAsync();
            Genres = Genres.OrderBy(x => x.Id).ToList();
            return new List<Genre>(Genres);
        }

        public static async Task<ObservableCollection<SearchMovie>> GetAllMovies(int pageNumber)
        {
            if (client == null)
              await  Init();
            searchMoviesContainer = await client.DiscoverMoviesAsync().Query(pageNumber);
            return new ObservableCollection<SearchMovie>(searchMoviesContainer.Results);
        }

        public static async Task<ObservableCollection<SearchMovie>> GetAllMoviesOfGenre(List<Genre> Genres)
        {
            if (client == null)
              await Init();
            await Task.Delay(2000);
            searchMoviesContainer = await client.DiscoverMoviesAsync().IncludeWithAllOfGenre(Genres).Query();
            return new ObservableCollection<SearchMovie>(searchMoviesContainer.Results);
        }

        public static async Task<Movie> GetMovie(int MovieId)
        {
            return await client.GetMovieAsync(MovieId, MovieMethods.Images);
        }
    }


}
