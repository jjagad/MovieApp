using MovieApp.ViewModel;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MovieApp.ViewModels
{
    public class FilterPopupViewModel : ViewModelBase
    {
        //Navigation
        public INavigation Navigation;
        private MoviesHomeViewModel _moviesHomeViewModel;
        private string SelectedCommand;

        public FilterPopupViewModel(MoviesHomeViewModel moviesHomeViewModel)
        {
            _moviesHomeViewModel = moviesHomeViewModel;
        }

        //Commands

        public ICommand FilterCommand => new Command(FilterClickAsync);
        public ICommand MostRecentCommand => new Command(MostRecentClickAsync);
        public ICommand HighRatedCommand => new Command(HighRatedClickAsync);
        public ICommand CancelCommand => new Command(CancelClick);
        public ICommand ApplyCommand => new Command(ApplyClick);
        

        private void CancelClick(object obj)
        {
            this.Navigation.PopPopupAsync();
        }

        private void ApplyClick()
        {
            this.Navigation.PopPopupAsync();
            if(SelectedCommand == "HighRatedCommand")
            {
                _moviesHomeViewModel.FilterByRatingsCommand.Execute(this);

            }
            else
            {
                _moviesHomeViewModel.FilterByMostRecentCommand.Execute(this);
            }
        }

        //methods
        private void HighRatedClickAsync(object obj)
        {
            SelectedCommand = "HighRatedCommand";
           
        }


        private void MostRecentClickAsync(object obj)
        {
            SelectedCommand = "MostRecentCommand";
        }

       
        private void FilterClickAsync(object obj)
        {
           
        }
    }
}
