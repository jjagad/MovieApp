using MovieApp.ViewModel;
using MovieApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPopup : PopupPage
    {
        public FilterPopup(MoviesHomeViewModel moviesHomeViewModel)
        {
            InitializeComponent();
            BindingContext = new FilterPopupViewModel(moviesHomeViewModel);
        }
    }
}