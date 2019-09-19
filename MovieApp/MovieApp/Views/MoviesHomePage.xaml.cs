using System;
using System.Collections.Generic;
using MovieApp.ViewModel;
using Xamarin.Forms;
using FFImageLoading.Forms;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.General;
using Acr.UserDialogs;

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
               
                ViewModel = new MoviesHomeViewModel
                {
                    Navigation = Navigation
                };
                this.BindingContext = ViewModel;

                CreateHorizontalScrollView();
            }
            catch (Exception ex)
            {

            }
        }

       

        /// <summary>
        /// This will create a horizontal scroll view for Genres
        /// </summary>
        public async void CreateHorizontalScrollView()
        {
            var Genres = await ViewModel.PopulateGenreList();

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

            foreach (var item in Genres)
            {
                button = new Button
                {
                    Text = item.Name,
                    BackgroundColor = Color.Transparent,
                    FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
                    CornerRadius = 10,
                    HeightRequest = 80,
                    TextColor = Color.Gray,
                    CommandParameter = item,
                    VerticalOptions = LayoutOptions.Start
                };

                button.Clicked += Button_Clicked;


                GenreStack.Children.Add(button);
            }
        }

        /// <summary>
        /// This will populate the movies list again 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonAll_Clicked(object sender, EventArgs e)
        {
           
            using (UserDialogs.Instance.Loading("Loading", null, null, true, MaskType.Black))
            {
                ResetSelection();
                ViewModel.IsGenreSelected = false;
                ViewModel.pageNumber = 1;
                var button = sender as Button;
                button.TextColor = Color.Blue;

                await ViewModel.PopulateMovieList();
            }                           

        }

        /// <summary>
        /// This will handle the click of a genre and filter the 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading("Loading", null, null, true, MaskType.Black))
            {
                ResetSelection();
                ViewModel.IsGenreSelected = true;
                ViewModel.pageNumber = 1;
                var button = sender as Button;

                button.TextColor = Color.Blue;

                var id = button.CommandParameter;
                ViewModel.FilterByGenreCommand.Execute(id);
            }         
        }

        /// <summary>
        /// This is to reset the Genre Selection 
        /// </summary>
        public void ResetSelection()
        {
            var list = GenreStack.Children;
            foreach (var item in list)
            {
                var btn = item as Button;
                btn.TextColor = Color.Gray;
            }
        }
        /// <summary>
        /// This will handle the movie item selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ViewModel.NavigateToDetailCommand.Execute(e.Item as SearchMovie);
        }

        /// <summary>
        /// This is implemented for pagination
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        async void OnItemAppearing(object Sender, ItemVisibilityEventArgs e)
        {
            var item = e.Item as SearchMovie;
            int index = ViewModel.ListOfMoviesToBeDisplayed.IndexOf(item);

            if (index != 0 && index == ViewModel.ListOfMoviesToBeDisplayed.Count - 1)
            {
                using (UserDialogs.Instance.Loading("Loading", null, null, true, MaskType.Black))
                {
                    ViewModel.pageNumber = ViewModel.pageNumber + 1;

                    await ViewModel.PopulateMovieListByPagination();
                }
                              
            }

        }
    }
}
