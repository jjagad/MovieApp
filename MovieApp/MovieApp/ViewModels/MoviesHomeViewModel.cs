using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MovieApp.Services;
using MovieApp.Views;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using Xamarin.Forms;

namespace MovieApp.ViewModel
{
    public class MoviesHomeViewModel : ViewModelBase
    {
        //Navigation
        public INavigation Navigation;
        private List<SearchMovie> MovieList;
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

       

        //Commands
        public ICommand NavigateToDetailCommand => new Command<SearchMovie>(HandleNavigateToDetailAsync);
        public ICommand SearchCommand => new Command(HandleSearchAsync);
        public ICommand FilterByGenreCommand => new Command<Genre>(FilterByGenreAsync);

       

        //Constructor
        public MoviesHomeViewModel()
        {
            PopulateMovieList();

        }

        public static void PopulateGenres()
        {
            var GenresList = DataService.Genres;
        }

        public async void PopulateMovieList()
        {
            ListOfMoviesToBeDisplayed = await DataService.GetAllMovies();
            
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

        public void HandleSearchAsync()
        {
            Navigation.PushModalAsync(new MoviesDetailsPage(353081));
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