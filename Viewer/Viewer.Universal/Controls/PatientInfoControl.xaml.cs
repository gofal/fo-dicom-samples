using Viewer.Library.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Viewer.Universal.Controls
{
    public sealed partial class PatientInfoControl : UserControl
    {
        public PatientInfoControl()
        {
            this.InitializeComponent();
        }

        public PatientModel Patient
        {
            get { return (PatientModel)GetValue(PatientProperty); }
            set { SetValue(PatientProperty, value); }
        }

        public static readonly DependencyProperty PatientProperty =
            DependencyProperty.Register(nameof(Patient), typeof(PatientModel), typeof(PatientInfoControl), new PropertyMetadata(null));

    }
}
