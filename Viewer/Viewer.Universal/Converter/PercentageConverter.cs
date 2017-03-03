using System;
using Windows.UI.Xaml.Data;

namespace Viewer.Universal.Converter
{
    public class PercentageConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }

            var returnValue = System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter);
            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
