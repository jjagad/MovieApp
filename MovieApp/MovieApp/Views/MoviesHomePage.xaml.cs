using System;
using System.Collections.Generic;
using MovieApp.ViewModel;
using Xamarin.Forms;
using FFImageLoading.Forms;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.General;

namespace MovieApp.Views
{
    public partial class MoviesHomePage : ContentPage
    {
        MoviesHomeViewModel ViewModel;
        public MoviesHomePage()
        {
            try
            {
                InitializeComponent();
                ViewModel = new MoviesHomeViewModel
                {
                    Navigation = Navigation
                };
                this.BindingContext = ViewModel;

                CreateHorizontalScrollView();
            }
            catch(Exception ex)
            {

            }
        }

        public async void CreateHorizontalScrollView()
        {
            var Genres =  await ViewModel.PopulateGenreList();

            Button button;

            Button buttonAll = new Button()
            {
                Text = "All",
                BackgroundColor = Color.Transparent,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                CornerRadius = 10,
                HeightRequest = 80,
                TextColor = Color.Blue,

            };

            buttonAll.Clicked += ButtonAll_Clicked;

            GenreStack.Children.Add(buttonAll);

            foreach(var item in Genres)
            {
                button = new Button
                {
                    Text = item.Name,
                    BackgroundColor = Color.Transparent,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                    CornerRadius = 10,
                    HeightRequest = 80,
                    TextColor = Color.Blue,
                    CommandParameter = item
                };

                button.Clicked += Button_Clicked;


                GenreStack.Children.Add(button);
            }
        }

        private void ButtonAll_Clicked(object sender, EventArgs e)
        {
            ViewModel.PopulateMovieList();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var id = button.CommandParameter;
            ViewModel.FilterByGenreCommand.Execute(id);
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ViewModel.NavigateToDetailCommand.Execute(e.Item as SearchMovie);
        }

    
        private void Handle_GenreTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            ViewModel.FilterByGenreCommand.Execute(e.ItemData as Genre);
        }
    }
}
