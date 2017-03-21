using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Viewer.Library.Tools;
using Viewer.Library.ViewModels;
using Windows.ApplicationModel.DataTransfer;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Dicom;
using Dicom.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Viewer.Universal.Views
{
    public sealed partial class FolderView : UserControl
    {
        public FolderView()
        {
            this.InitializeComponent();
            this.DataContextChanged += FolderControl_DataContextChanged;
        }

        private void FolderControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue != null)
                if (args.NewValue is INotifyPropertyChanged)
                {
                    INotifyPropertyChanged notify = args.NewValue as INotifyPropertyChanged;
                    notify.PropertyChanged += Notify_PropertyChanged;
                }
        }

        private void Notify_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CurrentImageCanvas.Invalidate();
            // todo: react on change of CurrentImage
        }

        public void Grid_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.MouseWheelDelta > 0)
                CurrentImageIndex.Value -= 1;
            else
                CurrentImageIndex.Value += 1;
        }

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.Text))
            {
                e.AcceptedOperation = DataPackageOperation.Copy;
                e.DragUIOverride.Caption = "Serie anzeigen";
            }
            else
                e.AcceptedOperation = DataPackageOperation.None;
        }

        private async void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.Text))
            {
                var data = await e.DataView.GetTextAsync();
                CurrentSeriesInstanceUID.Text = data;
            }
        }

        // since PointerMoved-event does not deliver a Delta, we have to memorize the last Position of the mouse
        private PointerPoint lastPoint;

        private void CurrentImage_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (lastPoint == null) // initial, his happens on the first PointerMoved-Handler
            { lastPoint = e.GetCurrentPoint(CurrentImageCanvas); return; }
            var pt = e.GetCurrentPoint(CurrentImageCanvas);

            bool move = (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse && pt.Properties.IsMiddleButtonPressed) ||
                   e.Pointer.PointerDeviceType == PointerDeviceType.Touch;
            // move only if the event comes from a touch or when the event comes from a mouse with pressed left button
            if (move)
            {
                var fvm = (FolderViewModel)(this.DataContext);
                fvm.ApplyFunctionMove(pt.Position.X - lastPoint.Position.X, pt.Position.Y - lastPoint.Position.Y);
            }

            lastPoint = pt;
        }

        private void CurrentImage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // important for Touch-input
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Touch)
                lastPoint = e.GetCurrentPoint(CurrentImageCanvas);
        }

        private void CurrentImageCanvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var fvm = (FolderViewModel)(this.DataContext);
            var tic = DateTime.Now;
            IImage srcImage = fvm.CurrentImage;
            if (srcImage == null) return;
            WriteableBitmap srcBitmap = srcImage.AsWriteableBitmap();
            if (srcBitmap == null) return;
            try
            {
                var cbmp = CanvasBitmap.CreateFromBytes(args.DrawingSession, srcBitmap.PixelBuffer, srcBitmap.PixelWidth, srcBitmap.PixelHeight, Windows.Graphics.DirectX.DirectXPixelFormat.B8G8R8A8UIntNormalized);
                var sizer = new ImageSizer() { SourceWith = cbmp.Size.Width, SourceHeight = cbmp.Size.Height, DestinationWidth = CurrentImageCanvas.Size.Width, DestinationHeight = CurrentImageCanvas.Size.Height }; // todo: cache this ImageSizer
                args.DrawingSession.DrawImage(cbmp, new Rect(sizer.ResultingOffsetX, sizer.ResultingOffsetY, sizer.ResultingWidth, sizer.ResultingHeight));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                string message = ex.ToString();
            }
            var toc = DateTime.Now;
            args.DrawingSession.DrawText($"rendern {(toc - tic).TotalMilliseconds} ms", 0, 0, Windows.UI.Colors.Red);
        }

        private void CurrentImageCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine($"Size: {e.NewSize.Width} x {e.NewSize.Height} (previous {e.PreviousSize.Width} x {e.PreviousSize.Height} )");
        }

        private void CurrentImageCanvas_CreateResources(CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
        }

    }
}
