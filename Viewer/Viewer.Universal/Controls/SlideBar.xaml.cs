using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Viewer.Universal.Controls
{
    public sealed partial class SlideBar : UserControl
    {
        public SlideBar()
        {
            this.InitializeComponent();
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(SlideBar), new PropertyMetadata(0.0, OnMinMaxPropertyChanged));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(SlideBar), new PropertyMetadata(1.0, OnMinMaxPropertyChanged));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(SlideBar), new PropertyMetadata(0.0, OnValuePropertyChanged));

        private static void OnMinMaxPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = (SlideBar)d;
            slider.SetThumbHeight();
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = (SlideBar)d;
            var newValue = (double)e.NewValue;

            if (newValue < slider.Minimum)
            {
                slider.Value = slider.Minimum;
            }
            else if (newValue > slider.Maximum)
            {
                slider.Value = slider.Maximum;
            }
            else
            {
                slider.Value = newValue;
            }

            slider.UpdateMainThumb(slider.Value);
        }

        public void UpdateMainThumb(double val, bool update = false)
        {
            if (ContainerCanvas != null)
            {
                if (update || !MainThumb.IsDragging)
                {
                    var relativeTop = ((val - Minimum) / (Maximum - Minimum + 1)) * ContainerCanvas.ActualHeight;

                    Canvas.SetTop(MainThumb, relativeTop);
                }
            }
        }

        private void ContainerCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var relativeTop = ((1) / (Maximum - Minimum + 1)) * ContainerCanvas.ActualHeight;

            Canvas.SetTop(MainThumb, relativeTop);

            SetThumbHeight();
        }

        public void SetThumbHeight()
        {
            MainThumb.Height = Math.Max(16, 1.0 / (Maximum - Minimum + 1) * ContainerCanvas.ActualHeight);
        }

        private void MainThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var val = DragThumb(MainThumb, 0, ContainerCanvas.ActualHeight, e.VerticalChange);
            UpdateMainThumb(val, true);
            Value = Math.Round(val);
        }

        private double DragThumb(Thumb thumb, double min, double max, double offset)
        {
            var currentPos = Canvas.GetTop(thumb);
            var nextPos = currentPos + offset;

            nextPos = Math.Max(min, nextPos);
            nextPos = Math.Min(max, nextPos);

            return (Minimum + (nextPos / ContainerCanvas.ActualHeight) * (Maximum - Minimum + 1));
        }

        private void MainThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            UpdateMainThumb(Value);
            Canvas.SetZIndex(MainThumb, 10);
        }

    }
}
