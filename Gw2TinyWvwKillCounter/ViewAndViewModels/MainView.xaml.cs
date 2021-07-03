using System.Windows;
using System.Windows.Input;

namespace Gw2TinyWvwKillCounter.ViewAndViewModels
{
    public partial class MainView : Window
    {
        public MainView()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
        }

        private void MainView_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}