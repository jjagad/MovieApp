using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using MovieApp.Services;

namespace MovieApp.Converters
{
    public class ImageSourceToURLConverterForHomePage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";
            string posterPath = value as string;
            posterPath = DataService.clientConfig.Images.BaseUrl + DataService.clientConfig.Images.LogoSizes[4] + posterPath;
            return posterPath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class ImageSourceToURLConverterForDetailsPage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";
            string posterPath = value as string;
            posterPath = DataService.clientConfig.Images.BaseUrl + DataService.clientConfig.Images.LogoSizes[6] + posterPath;
            return posterPath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
