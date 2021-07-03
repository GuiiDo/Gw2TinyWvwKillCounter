using System;
using System.Text.RegularExpressions;
using Gw2TinyWvwKillCounter.DefaultUiStuff;

namespace Gw2TinyWvwKillCounter.ViewAndViewModels
{
    public class SettingsDialogViewModel
    {
        public SettingsDialogViewModel(string apiKey, Action closeWindow)
        {
            ApiKey        = apiKey;
            _closeWindow  = closeWindow;
            SaveCommand   = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public string ApiKey
        {
            get => _apiKey;
            set => _apiKey = _invalidApiKeyCharacters.Replace(value, "");
        }

        public DialogResult DialogResult { get; set; } = DialogResult.Cancel;
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        private void Cancel()
        {
            DialogResult = DialogResult.Cancel;
            _closeWindow();
        }

        private void Save()
        {
            DialogResult = DialogResult.Save;
            _closeWindow();
        }

        private readonly Action _closeWindow;
        private string _apiKey;
        private static readonly Regex _invalidApiKeyCharacters = new Regex("[^a-zA-Z0-9-]");
    }
}