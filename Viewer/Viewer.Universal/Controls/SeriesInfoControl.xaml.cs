using Viewer.Library.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Viewer.Universal.Controls
{
    public sealed partial class SeriesInfoControl : UserControl
    {
        public SeriesInfoControl()
        {
            this.InitializeComponent();
        }

        public SeriesModel Serie
        {
            get { return (SeriesModel)GetValue(SerieProperty); }
            set { SetValue(SerieProperty, value); }
        }

        public static readonly DependencyProperty SerieProperty =
            DependencyProperty.Register(nameof(Serie), typeof(SeriesModel), typeof(SeriesInfoControl), new PropertyMetadata(null));

    }
}
