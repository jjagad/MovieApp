using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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
        public int pageNumber = 0;
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
                if(!string.IsNullOrEmpty(SearchText))
                {
                    ListOfMoviesToBeDisplayed = new ObservableCollection<SearchMovie>(ListOfMoviesToBeDisplayed.Where(m => m.OriginalTitle.ToLower().Contains(SearchText.ToLower())));
                }
                else
                {
                    pageNumber = 0;
                    Task.Run(async () => {
                        await PopulateMovieList();
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

      

        //Constructor
        public MoviesHomeViewModel()
        {
            PopulateMovieList();
                    
        }

        public static void PopulateGenres()
        {
            var GenresList = DataService.Genres;
        }

        public async Task PopulateMovieList()
        {
            ListOfMoviesToBeDisplayed = await DataService.GetAllMovies(pageNumber);
                       
           
        }


        private void FilterClickAsync()
        {
            Navigation.PushPopupAsync(new FilterPopup(this));
        }


        private void SearchIconClick()
        {
            IsSearchbarVisible = true;
            IsSearchIconVisible = false;
        }

        private void CancelIconClick(object obj)
        {
            IsSearchbarVisible = false;
            IsSearchIconVisible = true;
        }

        private void FilterByRatings()
        {
            ListOfMoviesToBeDisplayed = new ObservableCollection<SearchMovie>(ListOfMoviesToBeDisplayed.OrderByDescending(m => m.VoteAverage));
        }

        private void FilterByMostRecent()
        {
            ListOfMoviesToBeDisplayed = new ObservableCollection<SearchMovie>(ListOfMoviesToBeDisplayed.OrderByDescending(m => m.ReleaseDate));
        }


        public async Task<List<Genre>> PopulateGenreList()
        {
            ListOfGenresToBeDisplayed = await DataService.GetGenresListAsync();
            return ListOfGenresToBeDisplayed;
        }

        //Methods
        public void HandleNavigateToDetailAsync(SearchMovie movie)
        {
            Navigation.PushModalAsync(new MoviesDetailsPage(movie.Id));
        }
     

        private async void FilterByGenreAsync(Genre genre)
        {
            List<Genre> genreList = new List<Genre>();
            genreList.Add(genre);
            var list = await DataService.GetAllMoviesOfGenre(genreList);
            ListOfMoviesToBeDisplayed = list;
        }
    }
}