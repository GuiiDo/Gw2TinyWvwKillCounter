using System.Windows;
using System.Windows.Input;

namespace Gw2TinyWvwKillCounter.ViewAndViewModels
{
    public partial class ErrorDialogView : Window
    {
        public ErrorDialogView(string errorMessage)
        {
            // in code behind because dialog width didnt adjust to errorMessage otherwise
            DataContext = new ErrorDialogViewModel(errorMessage, Close);
            InitializeComponent();
        }

        private void DragWindowWithLeftMouseButton(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
