using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Gw2TinyWvwKillCounter.DefaultUiStuff;

namespace Gw2TinyWvwKillCounter.ViewAndViewModels
{
    public class SettingsDialogViewModel
    {
        public SettingsDialogViewModel(string apiKey, UiScaling uiScaling, Action closeWindow)
        {
            uiScaling.BackupUiScaling();

            ApiKey       = apiKey;
            UiScaling    = uiScaling;
            _closeWindow = closeWindow;

            SaveCommand   = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public UiScaling UiScaling { get; set; }

        public string ApiKey
        {
            get => _apiKey;
            set => _apiKey = _invalidApiKeyCharacters.Replace(value, "");
        }

        public DialogResult DialogResult { get; set; } = DialogResult.Cancel;
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public string UiScalingInPercent
        {
            get => UiScaling.ScalingInPercent.ToString();
            set => UiScaling.ScalingInPercent = uint.Parse(value);
        }
        public ObservableCollection<string> UiScalingInPercentValues { get; set; } = CreateValuesFrom50to500inStepsOf10();

        private static ObservableCollection<string> CreateValuesFrom50to500inStepsOf10()
        {
            var stringValues = Enumerable.Range(5, 46)
                                         .Select(i => i * 10)
                                         .Select(i => i.ToString());

            return new ObservableCollection<string>(stringValues);
        }

        private void Cancel()
        {
            UiScaling.RestoreUiScaling();
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