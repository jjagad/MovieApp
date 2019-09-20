using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MovieApp.Services;
using MovieApp.Views;
using Rg.Plugins.Popup.Extensions;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using Xamarin.Forms;

namespace MovieApp.ViewModel
{
    public class MoviesHomeViewModel : ViewModelBase
    {
        //Navigation
        public INavigation Navigation;
        public int pageNumber = 1;
        public bool IsGenreSelected = false;
        private Genre SelectedGenre;
        List<Genre> genreList;
        //Properties

        /// <summary>The list of Movies to be Displayed in the Movies Home Page." </summary>
        private ObservableCollection<SearchMovie> _listOfMoviesToBeDisplayed = new ObservableCollection<SearchMovie>();
        public ObservableCollection<SearchMovie> ListOfMoviesToBeDisplayed
        {
            get { return _listOfMoviesToBeDisplayed; }
            set { SetProperty(ref _listOfMoviesToBeDisplayed, value); }
        }

        /// <summary>The list of Genres to be Displayed in the Movies Home Page." </summary>
        private List<Genre> _listOfGenresToBeDisplayed = new List<Genre>();
        public List<Genre> ListOfGenresToBeDisplayed
        {
            get { return _listOfGenresToBeDisplayed; }
            set { SetProperty(ref _listOfGenresToBeDisplayed, value); }
        }

        /// <summary>
        /// Search text for Search bar on Movies Page 
        /// </summary>
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                SetProperty(ref _searchText, value);
                if (string.IsNullOrEmpty(SearchText))
                {
                    pageNumber = 0;
                    if (IsGenreSelected)
                    {
                        FilterByGenreAsync(SelectedGenre);
                    }

                    Task.Run(() =>
                    {
                        PopulateMovieList();
                    });
                }
               
            }
        }

        private bool _isSearchbarVisible = false;
        public bool IsSearchbarVisible
        {
            get { return _isSearchbarVisible; }
            set
            {
                SetProperty(ref _isSearchbarVisible, value);
            }
        }

        private bool _isSearchIconVisible = true;
        public bool IsSearchIconVisible
        {
            get { return _isSearchIconVisible; }
            set
            {
                SetProperty(ref _isSearchIconVisible, value);
            }
        }



        //Commands
        public ICommand NavigateToDetailCommand => new Command<SearchMovie>(HandleNavigateToDetailAsync);
        public ICommand FilterByGenreCommand => new Command<Genre>(FilterByGenreAsync);
        public ICommand FilterCommand => new Command(FilterClickAsync);
        public ICommand FilterByRatingsCommand => new Command(FilterByRatings);
        public ICommand FilterByMostRecentCommand => new Command(FilterByMostRecent);
        public ICommand SearchIconTappedCommand => new Command(SearchIconClick);
        public ICommand CancelTappedCommand => new Command(CancelIconClick);
        public ICommand SearchMoviesCommand => new Command(SearchMoviesAsync);


        //Constructor
        public MoviesHomeViewModel()
        {            
            PopulateMovieList();                                          
        }


        //Methods


        private void SearchMoviesAsync()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                ListOfMoviesToBeDisplayed = new ObservableCollection<SearchMovie>(ListOfMoviesToBeDisplayed.Where(m => m.OriginalTitle.ToLower().Contains(SearchText.ToLower())));
            }
            else
            {
                pageNumber = 0;
                if (IsGenreSelected)
                {
                    FilterByGenreAsync(SelectedGenre);
                }

                Task.Run(() => {
                    PopulateMovieList();
                });


            }
        }

        /// <summary>
        /// This will populate Genres
        /// </summary>
        public static void PopulateGenres()
        {
            var GenresList = DataService.Genres;
        }

        /// <summary>
        /// This is to populate movies list
        /// </summary>
        /// <returns></returns>
        public async void PopulateMovieList()
        {
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                using (UserDialogs.Instance.Loading("Loading", null, null, true, MaskType.Black))
                {
                    ListOfMoviesToBeDisplayed = await DataService.GetAllMovies(pageNumber);
                }
            }
            else
            {
                UserDialogs.Instance.Alert("Please check your internet connection");
            }
        }

        /// <summary>
        /// This is to populate movies with pagination applied
        /// </summary>
        /// <returns></returns>
        public async Task PopulateMovieListByPagination()
        {
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                if (IsGenreSelected)
                {
                    var movieList = await DataService.GetAllMoviesOfGenre(genreList, pageNumber);
                    AddItemToCollection(movieList);
                }
                else
                {
                    var movieList = await DataService.GetAllMovies(pageNumber);
                    AddItemToCollection(movieList);

                }
            }
            else
            {
                UserDialogs.Instance.Alert("Please check your internet connection!");
            }
        }

        public void AddItemToCollection(ObservableCollection<SearchMovie> movieList)
        {
            if (movieList != null)
            {
                foreach (var item in movieList)
                {
                    ListOfMoviesToBeDisplayed.Add(item);
                }
            }
        }

        /// <summary>
        /// This will open a filter popup
        /// </summary>
        private async void FilterClickAsync()
        {
          await Navigation.PushPopupAsync(new FilterPopup(this));
        }

        /// <summary>
        /// This will adjust the visibility of controls in toolbar
        /// </summary>
        private void SearchIconClick()
        {
            IsSearchbarVisible = true;
            IsSearchIconVisible = false;
        }

        /// <summary>
        /// This will adjust the visibility of controls in toolbar
        /// </summary>
        /// <param name="obj"></param>
        private void CancelIconClick(object obj)
        {
            IsSearchbarVisible = false;
            IsSearchIconVisible = true;
        }

        /// <summary>
        /// This will filter the movie list by ratings
        /// </summary>
        private void FilterByRatings()
        {
            ListOfMoviesToBeDisplayed = new ObservableCollection<SearchMovie>(ListOfMoviesToBeDisplayed.OrderByDescending(m => m.VoteAverage));
        }

        /// <summary>
        /// This will filter the movie list by Release Date
        /// </summary>
        private void FilterByMostRecent()
        {
            ListOfMoviesToBeDisplayed = new ObservableCollection<SearchMovie>(ListOfMoviesToBeDisplayed.OrderByDescending(m => m.ReleaseDate));
        }

        /// <summary>
        /// This will populate a fresh genre list
        /// </summary>
        /// <returns></returns>
        public async Task<List<Genre>> PopulateGenreList()
        {
            ListOfGenresToBeDisplayed = await DataService.GetGenresListAsync();
            return ListOfGenresToBeDisplayed;
        }

       /// <summary>
       /// This will navigate to movie details page for a selected movie
       /// </summary>
       /// <param name="movie"></param>
        public async void HandleNavigateToDetailAsync(SearchMovie movie)
        {
           await Navigation.PushModalAsync(new MoviesDetailsPage(movie.Id));
        }
     
        /// <summary>
        /// This will filter the movie list by genre selected
        /// </summary>
        /// <param name="genre"></param>
        private async void FilterByGenreAsync(Genre genre)
        {
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                using (UserDialogs.Instance.Loading("Loading", null, null, true, MaskType.Black))
                {
                    SelectedGenre = genre;
                    genreList = new List<Genre>();
                    genreList.Add(genre);
                    var list = await DataService.GetAllMoviesOfGenre(genreList);
                    ListOfMoviesToBeDisplayed = list;
                }
            }
            else
            {
                UserDialogs.Instance.Alert("Please check your internet connection");
            }
        }
    }
}