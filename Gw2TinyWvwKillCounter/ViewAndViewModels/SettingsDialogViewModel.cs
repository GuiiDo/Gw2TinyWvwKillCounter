using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Gw2TinyWvwKillCounter.Api;
using Gw2TinyWvwKillCounter.UiCommon;

namespace Gw2TinyWvwKillCounter.ViewAndViewModels
{
    public class SettingsDialogViewModel : PropertyChangedBase
    {
        public SettingsDialogViewModel(UiSettings uiSettings, Action closeWindow)
        {
            InitializeCommands();

            uiSettings.BackupUiSettings();
            UiSettings    = uiSettings;
            _closeWindow = closeWindow;

            var apiKeys = ApiKeyService.PersistedApiKeys;

            foreach (var apiKey in apiKeys)
                ApiKeys.Add(apiKey);

            if (apiKeys.Any())
                SelectedApiKey = ApiKeyService.PersistedSelectedApiKeyExists 
                    ? apiKeys.Single(a => a.Equals(ApiKeyService.PersistedSelectedApiKey))
                    : apiKeys.First();
        }

        public string SelectedApiKeyString
            => SelectedApiKey?.Value == null
                ? string.Empty
                : $"Key: {SelectedApiKey.Value}";

        public ApiKey SelectedApiKey
        {
            get => _selectedApiKey;
            set
            {
                Set(ref _selectedApiKey, value);
                RaisePropertyChanged(nameof(SelectedApiKeyString));
            }
        }

        public bool ApiKeyValueHasBeenChanged { get; set; }
        public ObservableCollection<ApiKey> ApiKeys { get; } = new ObservableCollection<ApiKey>();
        public bool ApiKeysComboBoxAndDeleteButtonAreVisible => ApiKeys.Any();
        public UiSettings UiSettings { get; }
        public DialogResult DialogResult { get; set; } = DialogResult.Cancel;

        private void AddApiKey()
        {
            var newApiKey   = new ApiKey();
            var apiKeyNames = ApiKeys.Select(a => a.Name).ToList();

            var addApiKeyDialogView      = new AddApiKeyDialogView();
            var addApiKeyDialogViewModel = new AddApiKeyDialogViewModel(newApiKey, apiKeyNames, UiSettings, () => addApiKeyDialogView.Close());
            addApiKeyDialogView.DataContext = addApiKeyDialogViewModel;
            addApiKeyDialogView.ShowDialog();

            if (addApiKeyDialogViewModel.DialogResult == DialogResult.Cancel)
                return;

            ApiKeys.Add(newApiKey);
            SelectedApiKey = newApiKey;
            RaisePropertyChanged(nameof(ApiKeysComboBoxAndDeleteButtonAreVisible));
            RaisePropertyChanged(nameof(SelectedApiKeyString));
        }

        private void RemoveApiKey()
        {
            // todo: keiner da: empty key setzen? -> vorsicht, sucht dann in empty list nach empty key oder? noch etwas unlogisch...

            if (SelectedApiKey == null)
                return;

            var dialogResult = MessageBox.Show($"Remove {SelectedApiKey.Name}?", "", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.No)
                return;

            ApiKeys.Remove(SelectedApiKey);
            SelectedApiKey = ApiKeys.FirstOrDefault();
            RaisePropertyChanged(nameof(ApiKeysComboBoxAndDeleteButtonAreVisible));
            RaisePropertyChanged(nameof(SelectedApiKeyString));
        }

        private void Cancel()
        {
            UiSettings.RestoreUiSettings();
            DialogResult = DialogResult.Cancel;
            _closeWindow();
        }

        private void Save()
        {
            var selectedApiKey = SelectedApiKey ?? new ApiKey();
            ApiKeyValueHasBeenChanged             = ApiKeyService.PersistedSelectedApiKey.Value != selectedApiKey.Value;
            ApiKeyService.PersistedSelectedApiKey = selectedApiKey;
            ApiKeyService.PersistedApiKeys        = ApiKeys.ToList();

            DialogResult = DialogResult.Save;
            _closeWindow();
        }

        private void InitializeCommands()
        {
            AddApiKeyCommand    = new RelayCommand(AddApiKey);
            RemoveApiKeyCommand = new RelayCommand(RemoveApiKey);
            SaveCommand         = new RelayCommand(Save);
            CancelCommand       = new RelayCommand(Cancel);
        }

        public RelayCommand RemoveApiKeyCommand { get; set; }
        public RelayCommand AddApiKeyCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        private readonly Action _closeWindow;
        private ApiKey _selectedApiKey;
    }
}