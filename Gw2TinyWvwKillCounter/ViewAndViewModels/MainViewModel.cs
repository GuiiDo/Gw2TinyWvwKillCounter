using System;
using System.Linq;
using Gw2TinyWvwKillCounter.DefaultUiStuff;
using Gw2TinyWvwKillCounter.Properties;
using Gw2TinyWvwKillCounter.Services;

namespace Gw2TinyWvwKillCounter.ViewAndViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        public MainViewModel()
        {
            ResetKillsAndDeathsCommand =  new RelayCommand(ResetKillsAndDeaths);
            OpenSettingsCommand        =  new RelayCommand(OpenSettings);
            OnWindowLoadedCommand      =  new RelayCommand(OnWindowLoaded);
            OnWindowClosingCommand     =  new RelayCommand(OnWindowClosing);
            _asyncTimer.IntervalEnded  += OnAsyncTimerIntervalEnded;
        }

        public string Test // todo weg
        {
            get => _test;
            set => Set(ref _test, value);
        }

        public string KillsPerIntervalLog // todo weg
        {
            get => _killsPerIntervalLog;
            set => Set(ref _killsPerIntervalLog, value);
        }

        public string TotalKillsTooltip
        {
            get => _totalKillsTooltip;
            set => Set(ref _totalKillsTooltip, value);
        }

        public string TotalDeathsTooltip
        {
            get => _totalDeathsTooltip;
            set => Set(ref _totalDeathsTooltip, value);
        }

        public int TotalKills
        {
            get => _totalKills;
            set
            {
                _totalKills       = value;
                TotalKillsTooltip = $"{value} total kills (all characters)";
            }
        }

        public int TotalDeaths
        {
            get => _totalDeaths;
            set
            {
                _totalDeaths       = value;
                TotalDeathsTooltip = $"{value} total deaths (all characters)";
            }
        }

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
        private void OnWindowClosing() => Settings.Default.Save();

        private async void OpenSettings()
        {
            ResetButtonIsEnabled = false;

            var settingsDialogView      = new SettingsDialogView();
            var settingsDialogViewModel = new SettingsDialogViewModel(Settings.Default.ApiKey, () => settingsDialogView.Close());
            settingsDialogView.DataContext = settingsDialogViewModel;
            settingsDialogView.ShowDialog();

            if (settingsDialogViewModel.DialogResult == DialogResult.Cancel)
                return;

            Settings.Default.ApiKey = settingsDialogViewModel.ApiKey;
            Settings.Default.Save();

            if (await ApiKeyService.ApiKeyIsInvalid(Settings.Default.ApiKey))
            {
                _asyncTimer.Stop();
                return;
            }

            (TotalKills, TotalDeaths) = await _killDeathService.InitialiseAndGetTotalKillsDeath(Settings.Default.ApiKey);
            KillsPerIntervalLog       = AddLogLineAndTruncateLogIfItGetsTooLong(KillsPerIntervalLog); // todo weg
            Test                      = _killDeathService.Test;                                       // todo weg

            ResetKillsAndDeaths();
            _asyncTimer.Start();

            ResetButtonIsEnabled = true;
        }

        private void ResetKillsAndDeaths()
        {
            _killDeathService.ResetKillsAndDeaths();
            KillsSinceReset  = 0;
            DeathsSinceReset = 0;
        }

        private async void OnWindowLoaded()
        {
            ResetButtonIsEnabled = false;
            if (await ApiKeyService.ApiKeyIsInvalid(Settings.Default.ApiKey))
                return;

            (TotalKills, TotalDeaths) = await _killDeathService.InitialiseAndGetTotalKillsDeath(Settings.Default.ApiKey);
            KillsPerIntervalLog       = AddLogLineAndTruncateLogIfItGetsTooLong(KillsPerIntervalLog); // todo weg
            Test                      = _killDeathService.Test;                                       // todo weg

            _asyncTimer.Start();
            ResetButtonIsEnabled = true;
        }

        private async void OnAsyncTimerIntervalEnded(object sender, EventArgs e)
        {
            (KillsSinceReset, DeathsSinceReset, TotalKills, TotalDeaths) = await _killDeathService.GetKillsAndDeathsSinceReset();
            Test                                                         = _killDeathService.Test;                                       // todo weg
            KillsPerIntervalLog                                          = AddLogLineAndTruncateLogIfItGetsTooLong(KillsPerIntervalLog); // todo weg
        }

        private string AddLogLineAndTruncateLogIfItGetsTooLong(string log) // todo weg
        {
            var logLines = _killsPerIntervalLog.Split('\n').ToList();
            if (logLines.Count > 12)
            {
                logLines.Remove(logLines.Last());
                log = string.Join('\n', logLines);
            }

            var nexLogLine = $"{DateTime.Now:HH:mm} {TotalKills} {KillsSinceReset}\n";
            return nexLogLine + log;
        }

        private const int API_REQUEST_INTERVAL_IN_SECONDS = 5 * 60;
        private readonly AsyncTimer _asyncTimer = new AsyncTimer(API_REQUEST_INTERVAL_IN_SECONDS);
        private readonly KillDeathService _killDeathService = new KillDeathService();
        private readonly ShowDialogWithUnhandledExceptionService _showDialogWithUnhandledExceptionService = new ShowDialogWithUnhandledExceptionService();
        private int _killsSinceReset;
        private int _deathsSinceReset;
        private int _totalDeaths;
        private int _totalKills;
        private string _test;
        private bool _resetButtonIsEnabled = true;
        private string _killsPerIntervalLog = string.Empty;
        private string _totalKillsTooltip;
        private string _totalDeathsTooltip;
    }
}