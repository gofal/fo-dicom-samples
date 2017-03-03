using System.ComponentModel;
using Viewer.Universal.Controls;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Core;
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
            // this.DataContextChanged += MainView_DataContextChanged;
        }


        //private void MainView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        //{
        //    if (args.NewValue != null)
        //        if (args.NewValue is INotifyPropertyChanged)
        //        {
        //            INotifyPropertyChanged notify = args.NewValue as INotifyPropertyChanged;
        //            notify.PropertyChanged += Notify_PropertyChanged;
        //        }
        //}


        //private void Notify_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "AsyncPatients")
        //    {
        //        var d = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
        //        d.RunAsync(CoreDispatcherPriority.Normal, () =>
        //        {
        //            PatientsList.UpdateLayout();
        //        });
        //    }
        //    // todo: react on change of CurrentImage
        //}


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
