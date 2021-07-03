using System;
using Gw2TinyWvwKillCounter.DefaultUiStuff;
using Gw2TinyWvwKillCounter.Properties;
using Gw2TinyWvwKillCounter.Services;

namespace Gw2TinyWvwKillCounter.ViewAndViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        public MainViewModel()
        {
            ResetKillsAndDeathsCommand = new RelayCommand(ResetKillsAndDeaths);
            OpenSettingsCommand        = new RelayCommand(OpenSettings);
            OnWindowLoadedCommand      = new RelayCommand(OnWindowLoaded);
            OnWindowClosingCommand     = new RelayCommand(OnWindowClosing);
        }

        public string Test // todo weg
        {
            get => _test;
            set => Set(ref _test, value);
        }

        public string TotalKillsTooltip => $"{_totalKills} total kills (all characters)";
        public string TotalDeathsTooltip => $"{_totalDeaths} total deaths (all characters)";

        public int KillsSinceReset
        {
            get => _killsSinceReset;
            set => Set(ref _killsSinceReset, value);
        }

        public int DeathsSinceReset
        {
            get => _deathsSinceReset;
            set => Set(ref _deathsSinceReset, value);
        }

        public bool ResetButtonIsEnabled
        {
            get => _resetButtonIsEnabled;
            set => Set(ref _resetButtonIsEnabled, value);
        }

        public RelayCommand OnWindowLoadedCommand { get; set; }
        public RelayCommand ResetKillsAndDeathsCommand { get; set; }
        public RelayCommand OpenSettingsCommand { get; set; }
        public RelayCommand OnWindowClosingCommand { get; set; }

        private void OnWindowClosing()
        {
            Settings.Default.Save();
        }

        private async void OpenSettings()
        {
            var settingsDialogView      = new SettingsDialogView();
            var settingsDialogViewModel = new SettingsDialogViewModel(Settings.Default.ApiKey, () => settingsDialogView.Close());
            settingsDialogView.DataContext = settingsDialogViewModel;
            settingsDialogView.ShowDialog();

            if (settingsDialogViewModel.DialogResult == DialogResult.Cancel)
                return;

            Settings.Default.ApiKey = settingsDialogViewModel.ApiKey;
            Settings.Default.Save();

            if (await ApiKeyValidation.ApiKeyIsInvalid(Settings.Default.ApiKey))
                return;

            ResetButtonIsEnabled        = false;
            (_totalKills, _totalDeaths) = await _killDeathService.Initialise(Settings.Default.ApiKey);
            OnPropertyChanged(nameof(TotalKillsTooltip));
            OnPropertyChanged(nameof(TotalDeathsTooltip));
            ResetKillsAndDeaths();
            ResetButtonIsEnabled = true;
        }

        private async void ResetKillsAndDeaths()
        {
            ResetButtonIsEnabled = false;

            _asyncTimer.Stop();

            await _killDeathService.ResetKillsAndDeaths();
            KillsSinceReset  = 0;
            DeathsSinceReset = 0;

            _asyncTimer.Start(INTERVAL_IN_SECONDS);
            ResetButtonIsEnabled = true;
        }

        private async void OnWindowLoaded()
        {
            ResetButtonIsEnabled = false;
            if (await ApiKeyValidation.ApiKeyIsInvalid(Settings.Default.ApiKey))
                return;

            (_totalKills, _totalDeaths) = await _killDeathService.Initialise(Settings.Default.ApiKey);
            OnPropertyChanged(nameof(TotalKillsTooltip));
            OnPropertyChanged(nameof(TotalDeathsTooltip));
            Test = _killDeathService.Test; // todo weg

            _asyncTimer.IntervalEnded += OnAsyncTimerIntervalEnded;
            _asyncTimer.Start(INTERVAL_IN_SECONDS);
            ResetButtonIsEnabled = true;
        }

        private async void OnAsyncTimerIntervalEnded(object sender, EventArgs e)
        {

            (KillsSinceReset, DeathsSinceReset, _totalKills, _totalDeaths) = await _killDeathService.GetKillsAndDeathsSinceReset();
            OnPropertyChanged(nameof(TotalKillsTooltip));
            OnPropertyChanged(nameof(TotalDeathsTooltip));
            Test = _killDeathService.Test; // todo weg
        }

        private const int INTERVAL_IN_SECONDS = 60 * 5;
        private readonly KillDeathService _killDeathService = new KillDeathService();
        private readonly AsyncTimer _asyncTimer = new AsyncTimer();
        private readonly ShowDialogWithUnhandledExceptionService _showDialogWithUnhandledExceptionService = new ShowDialogWithUnhandledExceptionService();
        private int _killsSinceReset;
        private int _deathsSinceReset;
        private int _totalDeaths;
        private int _totalKills;
        private string _test;
        private bool _resetButtonIsEnabled = true;
    }
}