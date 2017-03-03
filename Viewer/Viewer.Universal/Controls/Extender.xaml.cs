using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Viewer.Universal.Controls
{
    public sealed partial class Extender : UserControl
    {
        public Extender()
        {
            this.InitializeComponent();
        }

        public bool Expanded
        {
            get { return (bool)GetValue(ExpandedProperty); }
            set
            {
                SetValue(ExpandedProperty, value);
                SetValue(CollapsedProperty, !value);
            }
        }

        public static readonly DependencyProperty ExpandedProperty =
            DependencyProperty.Register(nameof(Expanded), typeof(bool), typeof(Extender), new PropertyMetadata(true));

        public bool Collapsed
        {
            get { return (bool)GetValue(CollapsedProperty); }
            set
            {
                SetValue(CollapsedProperty, value);
                SetValue(ExpandedProperty, !value);
            }
        }

        public static readonly DependencyProperty CollapsedProperty =
            DependencyProperty.Register(nameof(Collapsed), typeof(bool), typeof(Extender), new PropertyMetadata(false));


        public object HeaderTemplate
        {
            get { return (object)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(object), typeof(Extender), new PropertyMetadata(null));

        public object DataTemplate
        {
            get { return (object)GetValue(DataTemplateProperty); }
            set { SetValue(DataTemplateProperty, value); }
        }

        public static readonly DependencyProperty DataTemplateProperty =
            DependencyProperty.Register(nameof(DataTemplate), typeof(object), typeof(Extender), new PropertyMetadata(null));


        private void StackPanel_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // by clicking on the Canvas with the arrows in it, the user changes the Expanded-State
            this.Expanded = !this.Expanded;
        }

    }
}
