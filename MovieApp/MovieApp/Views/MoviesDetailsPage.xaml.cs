using System;
using System.Collections.Generic;
using MovieApp.ViewModel;
using Xamarin.Forms;

namespace MovieApp.Views
{
    public partial class MoviesDetailsPage : ContentPage
    {
        MoviesDetailViewModel ViewModel;

        public MoviesDetailsPage(int MovieId)
        {
            InitializeComponent();
            ViewModel = new MoviesDetailViewModel(MovieId)
            {
                Navigation = Navigation
            };
            this.BindingContext = ViewModel;
        }
    }
}
