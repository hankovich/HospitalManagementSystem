namespace Hms.UI.Views
{
    using System.Windows.Controls;

    using Hms.UI.ViewModels;

    using Ninject;

    /// <summary>
    /// Логика взаимодействия для MedicalCardView.xaml
    /// </summary>
    public partial class MedicalCardView : UserControl
    {
        public MedicalCardView()
        {
            InitializeComponent();
            this.DataContext = App.Kernel.Get<MedicalCardViewModel>();
        }
    }
}
