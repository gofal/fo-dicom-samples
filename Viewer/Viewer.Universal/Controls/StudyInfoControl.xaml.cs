using Viewer.Library.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Viewer.Universal.Controls
{
    public sealed partial class StudyInfoControl : UserControl
    {
        public StudyInfoControl()
        {
            this.InitializeComponent();
        }

        public StudyModel Study
        {
            get { return (StudyModel)GetValue(StudyProperty); }
            set { SetValue(StudyProperty, value); }
        }

        public static readonly DependencyProperty StudyProperty =
            DependencyProperty.Register(nameof(Study), typeof(StudyModel), typeof(StudyInfoControl), new PropertyMetadata(null));

    }
}
