namespace Hms.UI.Views
{
    using System.Windows.Controls;

    using Hms.UI.ViewModels;

    using Ninject;

    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            this.InitializeComponent();
            this.DataContext = App.Kernel.Get<MenuViewModel>();
        }
    }
}
