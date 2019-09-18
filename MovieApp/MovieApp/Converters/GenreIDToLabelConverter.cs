using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using TMDbLib.Objects;

namespace MovieApp.Converters
{
    public class GenreIDToLabelConverterForHomePage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";
            string result = string.Empty;
            List<int> listOfGenres = value as List<int>;
            int count = 3;
            if (listOfGenres.Count > 3)
                count = 3;
            else count = listOfGenres.Count;
            for (int i = 0; i < count; i++)
                result += Services.DataService.Genres.FirstOrDefault(x => x.Id == listOfGenres[i]).Name + " | ";
            
            if (!string.IsNullOrEmpty(result))
                result = result.Substring(0, result.Length - 3);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GenreIDToLabelConverterForDetailsPage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";
            string result = string.Empty;
            var listOfGenres = value as List<TMDbLib.Objects.General.Genre>;

            for (int i = 0; i < listOfGenres.Count; i++)
                result += Services.DataService.Genres.FirstOrDefault(x => x.Id == listOfGenres[i].Id).Name + " | ";
            
            if (!string.IsNullOrEmpty(result))
                result = result.Substring(0, result.Length - 3);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
