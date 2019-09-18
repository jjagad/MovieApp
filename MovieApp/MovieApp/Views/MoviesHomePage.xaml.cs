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
        private MoviesHomeViewModel ViewModel;
        

        public MoviesHomePage()
        {
            try
            {
                InitializeComponent();
                //NavigationPage.SetHasNavigationBar(this, false);
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
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
                CornerRadius = 10,
                HeightRequest = 80,
                TextColor = Color.Blue,
                VerticalOptions = LayoutOptions.Start
            };

            buttonAll.Clicked += ButtonAll_Clicked;

            GenreStack.Children.Add(buttonAll);

            foreach(var item in Genres)
            {
                button = new Button
                {
                    Text = item.Name,
                    BackgroundColor = Color.Transparent,
                    FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
                    CornerRadius = 10,
                    HeightRequest = 80,
                    TextColor = Color.Blue,
                    CommandParameter = item,
                    VerticalOptions = LayoutOptions.Start
                };

                button.Clicked += Button_Clicked;


                GenreStack.Children.Add(button);
            }
        }

        private async void ButtonAll_Clicked(object sender, EventArgs e)
        {
            ViewModel.pageNumber = 0;
            await ViewModel.PopulateMovieList();
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


        //async void OnItemAppearing(object Sender, ItemVisibilityEventArgs e)
        //{
        //    var item = e.Item as SearchMovie;
        //    int index = ViewModel.ListOfMoviesToBeDisplayed.IndexOf(item);
            
        //    if (index != 0 && index % 19 == 0)
        //    {

        //        if (previousindex != index)
        //        {
        //            ViewModel.pageNumber = ViewModel.pageNumber + 1;
        //            await ViewModel.PopulateMovieList();
        //            MoviesList.ScrollTo(ViewModel.ListOfMoviesToBeDisplayed[index + 3], ScrollToPosition.Start, true);
        //        }
        //        previousindex = index;
        //    }
            
        //}
    }
}
