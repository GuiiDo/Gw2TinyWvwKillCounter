using System.Windows;
using System.Windows.Input;

namespace Gw2TinyWvwKillCounter.ViewAndViewModels
{
    public partial class AddApiKeyDialogView : Window
    {
        public AddApiKeyDialogView()
        {
            InitializeComponent();
        }

        private void DragWindowWithLeftMouseButton(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}