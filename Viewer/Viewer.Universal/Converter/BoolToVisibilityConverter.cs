using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Viewer.Universal.Converter
{
    /// <summary>
    /// Converter for Databinding, used to convert a Bool Value to Visibility State
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }

            var isChecked = (bool)value;
            return isChecked ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

    }
}
