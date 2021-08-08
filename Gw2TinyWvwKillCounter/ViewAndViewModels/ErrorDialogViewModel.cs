using System;
using Gw2TinyWvwKillCounter.UiCommon;

namespace Gw2TinyWvwKillCounter.ViewAndViewModels
{
    public class ErrorDialogViewModel
    {
        public ErrorDialogViewModel(string errorMessage, Action closeWindow)
        {
            ConfirmCommand = new RelayCommand(closeWindow);
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
    }
}