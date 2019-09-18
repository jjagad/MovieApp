using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.Movies;
using Xamarin.Forms;
using MovieApp.Services;

namespace MovieApp.ViewModel
{
    public class MoviesDetailViewModel : ViewModelBase
    {
        //Navigation
        public INavigation Navigation;

        /// <summary>The Movie Detials to be Displayed in the Movies Details Page." </summary>
        private Movie _selectedMovie = new Movie();
        public Movie SelectedMovie
        {
            get { return _selectedMovie; }
            set { SetProperty(ref _selectedMovie, value); }
        }

        //Commands
        public ICommand CloseCommand => new Command(ClosePage);

        //Constructor
        public MoviesDetailViewModel(int MovieId)
        {
            SelectedMovie = Task.Run(async () => await DataService.GetMovie(MovieId)).Result;
        }

        //Methods
        public void ClosePage()
        {
            Navigation.PopModalAsync(true);
        }
    }
}