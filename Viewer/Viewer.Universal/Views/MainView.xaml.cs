using Viewer.Universal.Controls;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Viewer.Universal.Views
{
    public sealed partial class MainView : UserControl
    {

        public MainView()
        {
            this.InitializeComponent();
        }


        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FrameworkElement element = scrollViewer.Parent as FrameworkElement;
            scrollViewer.Width = element.ActualWidth * 40 / 100;
        }


        private void SeriesInfoControl_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            args.Data.RequestedOperation = DataPackageOperation.Copy;
            args.Data.SetText((string)((SeriesInfoControl)sender).Tag);
        }


    }
}
