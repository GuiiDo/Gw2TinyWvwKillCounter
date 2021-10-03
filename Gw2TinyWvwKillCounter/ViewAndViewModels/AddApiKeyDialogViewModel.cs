using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Gw2TinyWvwKillCounter.Api;
using Gw2TinyWvwKillCounter.UiCommon;

namespace Gw2TinyWvwKillCounter.ViewAndViewModels
{
    public class AddApiKeyDialogViewModel : PropertyChangedBase
    {
        public AddApiKeyDialogViewModel(ApiKey apiKey, List<string> apiKeyNames, UiSettings uiSettings, Action closeWindow)
        {
            InitializeCommands();

            _apiKey = apiKey;
            _apiKeyNames = apiKeyNames;
            UiSettings    = uiSettings;
            _closeWindow = closeWindow;

        }
        
        public string ApiKeyName
        {
            get => _apiKey.Name;
            set => _apiKey.Name = _invalidApiKeyNameCharacters.Replace(value, "");
        }

        public string ApiKeyAsString
        {
            get => _apiKey.Value;
            set => _apiKey.Value = _invalidApiKeyCharacters.Replace(value, "");
        }

        public UiSettings UiSettings { get; }
        public DialogResult DialogResult { get; set; } = DialogResult.Cancel;

        private void Cancel()
        {
            DialogResult = DialogResult.Cancel;
            _closeWindow();
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(ApiKeyName))
            {
                new ErrorDialogView("Name cannot be empty. Enter a name.").ShowDialog();
                return;
            }

            var nameIsAlreadyInUse = _apiKeyNames.Contains(ApiKeyName);
            if (nameIsAlreadyInUse)
            {
                new ErrorDialogView("Name is already used for another API key. Enter a different name.").ShowDialog();
                return;
            }

            if (string.IsNullOrWhiteSpace(ApiKeyAsString))
            {
                new ErrorDialogView("API key cannot be empty. Enter an API key.").ShowDialog();
                return;
            }

            DialogResult = DialogResult.Save;
            _closeWindow();
        }

        private void InitializeCommands()
        {
            SaveCommand              = new RelayCommand(Save);
            CancelCommand            = new RelayCommand(Cancel);
            OpenApiKeyWebsiteCommand = new RelayCommand(() => Process.Start("explorer.exe", "https://account.arena.net/applications"));
        }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand OpenApiKeyWebsiteCommand { get; set; }

        private readonly Action _closeWindow;
        private static readonly Regex _invalidApiKeyCharacters = new Regex("[^a-zA-Z0-9-]");
        private static readonly Regex _invalidApiKeyNameCharacters = new Regex("[^a-zA-Z0-9-. ]");
        private readonly ApiKey _apiKey;
        private readonly List<string> _apiKeyNames;
    }
}