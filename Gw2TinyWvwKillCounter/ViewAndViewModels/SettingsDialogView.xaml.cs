using System.Windows;
using System.Windows.Input;

namespace Gw2TinyWvwKillCounter.ViewAndViewModels
{
    public partial class SettingsDialogView : Window
    {
        public SettingsDialogView()
        {
            InitializeComponent();
        }

        private void SettingsView_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
