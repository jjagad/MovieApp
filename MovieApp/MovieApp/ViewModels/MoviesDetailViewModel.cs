using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbLib.Objects.Movies;
using Xamarin.Forms;
using MovieApp.Services;
using Acr.UserDialogs;

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
            try
            {
                
                ShowSelectedMovie(MovieId);
            }
            catch(Exception ex)
            {               
                UserDialogs.Instance.Alert("Something went wrong..Please try again!","OK");
                
            }
        }

        public async void ShowSelectedMovie(int MovieId)
        {
            try
            {
                using (UserDialogs.Instance.Loading("Loading", null, null, true, MaskType.Black))
                {
                    SelectedMovie = await DataService.GetMovie(MovieId);
                }
            }
            catch(Exception ex)
            {
                UserDialogs.Instance.Alert("Something went wrong..Please try again!");
            }
        }

        //Methods
        public async void ClosePage()
        {
           await Navigation.PopModalAsync(true);
        }
    }
}