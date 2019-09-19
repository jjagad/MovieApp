using Acr.UserDialogs;
using Newtonsoft.Json;
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

        /// <summary>
        /// This will fetch list of all genres
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Genre>> GetGenresListAsync()
        {
            try
            {
                Genres = await client.GetMovieGenresAsync();
                Genres = Genres.OrderBy(x => x.Id).ToList();
                return new List<Genre>(Genres);
            }
            catch(Exception ex)
            {
               // UserDialogs.Instance.Toast("Something went wrong..Please try again");
                UserDialogs.Instance.Alert("Something went wrong..Please try again");
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// This will fetch list of all movies
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public static async Task<ObservableCollection<SearchMovie>> GetAllMovies(int pageNumber)
        {
            try
            {
                if (client == null)
                    await Init();
                searchMoviesContainer = await client.DiscoverMoviesAsync().Query(pageNumber);              
                return new ObservableCollection<SearchMovie>(searchMoviesContainer.Results);
              
            }
            catch(Exception ex)
            {
                UserDialogs.Instance.Alert("Something went wrong..Please try again");
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// This will fetch list of movies based on genres
        /// </summary>
        /// <param name="Genres"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public static async Task<ObservableCollection<SearchMovie>> GetAllMoviesOfGenre(List<Genre> Genres,int pageNumber = 0)
        {
            try
            {
                if (client == null)
                    await Init();
               
                searchMoviesContainer = await client.DiscoverMoviesAsync().IncludeWithAllOfGenre(Genres).Query(pageNumber);
                return new ObservableCollection<SearchMovie>(searchMoviesContainer.Results);
            }
            catch(Exception ex)
            {
                UserDialogs.Instance.Alert("Something went wrong..Please try again");
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// This will fetch movie based on movie id
        /// </summary>
        /// <param name="MovieId"></param>
        /// <returns></returns>
        public static async Task<Movie> GetMovie(int MovieId)
        {
            return await client.GetMovieAsync(MovieId, MovieMethods.Images);
        }
    }


}
