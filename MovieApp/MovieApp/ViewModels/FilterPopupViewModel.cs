using MovieApp.Enums;
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
        private int SelectedCommand;

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

        /// <summary>
        /// This is to apply Genre Selection on Movies Page
        /// </summary>
        private void ApplyClick()
        {
            this.Navigation.PopPopupAsync();
            if(SelectedCommand == Convert.ToInt32(Filter.HighRated))
            {
                _moviesHomeViewModel.FilterByRatingsCommand.Execute(this);

            }
            else
            {
                _moviesHomeViewModel.FilterByMostRecentCommand.Execute(this);
            }
        }

       /// <summary>
       /// This is to assign selected option 
       /// </summary>
       /// <param name="obj"></param>
        private void HighRatedClickAsync(object obj)
        {
            SelectedCommand = Convert.ToInt32(Filter.HighRated);
           
        }

        /// <summary>
        /// This is to assign seleted option
        /// </summary>
        /// <param name="obj"></param>
        private void MostRecentClickAsync(object obj)
        {
            SelectedCommand = Convert.ToInt32(Filter.MostRecent);
        }

       
        private void FilterClickAsync(object obj)
        {
           
        }
    }
}
