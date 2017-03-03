using Dicom;
using Dicom.Log;
using System;
using Windows.UI.Xaml.Data;

namespace Viewer.Universal.Converter
{
    public class DicomFileToDumpConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }

            var dataset = value as DicomDataset;
            if (dataset == null)
            {
                throw new InvalidOperationException("Only DICOM files supported.");
            }

            var dump = dataset.WriteToString();
            return dump;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
