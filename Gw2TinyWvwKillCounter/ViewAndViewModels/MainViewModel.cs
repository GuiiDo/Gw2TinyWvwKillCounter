using System;
using System.Threading.Tasks;
using Gw2TinyWvwKillCounter.Api;
using Gw2TinyWvwKillCounter.Properties;
using Gw2TinyWvwKillCounter.UiCommon;

namespace Gw2TinyWvwKillCounter.ViewAndViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        public MainViewModel()
        {
            ShowDialogWithUnhandledExceptionService.InitializeExceptionHandling();
            InitializeCommands();

            _asyncTimer.IntervalEnded  += OnAsyncTimerIntervalEnded;
        }

        public UiSettings UiSettings { get; set; } = new UiSettings();

        public bool TitleBarButtonsAreVisible
        {
            get => _titleBarButtonsAreVisible;
            set => Set(ref _titleBarButtonsAreVisible, value);
        }

        public string KillsDeathsSummaryText
        {
            get => _killsDeathsSummaryText;
            set => Set(ref _killsDeathsSummaryText, value);
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

        private void OnWindowClosing() => Settings.Default.Save();

        private async void OpenSettingsWrapper()
        {
            ResetButtonIsEnabled = false;
            await OpenSettings();
            ResetButtonIsEnabled = true;
        }

        private async Task OpenSettings()
        {
            var settingsDialogView      = new SettingsDialogView();
            var settingsDialogViewModel = new SettingsDialogViewModel(UiSettings, () => settingsDialogView.Close());
            settingsDialogView.DataContext = settingsDialogViewModel;
            settingsDialogView.ShowDialog();

            if (settingsDialogViewModel.DialogResult == DialogResult.Cancel)
                return;

            var apiKeyIsValid = await ApiKeyValidationService.ApiKeyIsInvalid(ApiKeyService.PersistedSelectedApiKey.Value) == false;

            var apiKeyIsCurrentlyUsedToCallApi = apiKeyIsValid && settingsDialogViewModel.ApiKeyValueHasBeenChanged == false;
            if (apiKeyIsCurrentlyUsedToCallApi)
                return;

            await _asyncTimer.Stop();

            KillsSinceReset  = 0;
            DeathsSinceReset = 0;
            _totalKills      = 0;
            _totalDeaths     = 0;
            KillsDeathsSummaryText = string.Empty;

            if (apiKeyIsValid && settingsDialogViewModel.ApiKeyValueHasBeenChanged) // "&& ApiKeyValueHasBeenChanged" is not necessary but helps understanding the code.
            {
                (_totalKills, _totalDeaths) = await _killDeathService.InitialiseAndGetTotalKillsDeath(ApiKeyService.PersistedSelectedApiKey.Value);
                KillsDeathsSummaryText      = _killsDeathsSummaryTextService.ResetAndReturnSummaryText(_totalKills, _totalDeaths);

                _asyncTimer.Start();
            }
        }

        private void ResetKillsAndDeaths()
        {
            KillsSinceReset = 0;
            DeathsSinceReset = 0;
            _killDeathService.ResetKillsAndDeaths();
            KillsDeathsSummaryText = _killsDeathsSummaryTextService.ResetAndReturnSummaryText(_totalKills, _totalDeaths);
        }

        private async void OnWindowLoaded()
        {
            ResetButtonIsEnabled = false;
            await StartCallingApiIfApiKeyIsValid();
            ResetButtonIsEnabled = true;
        }

        private async Task StartCallingApiIfApiKeyIsValid()
        {
            if (await ApiKeyValidationService.ApiKeyIsInvalid(ApiKeyService.PersistedSelectedApiKey.Value))
                return;

            (_totalKills, _totalDeaths) = await _killDeathService.InitialiseAndGetTotalKillsDeath(ApiKeyService.PersistedSelectedApiKey.Value);
            KillsDeathsSummaryText      = _killsDeathsSummaryTextService.ResetAndReturnSummaryText(_totalKills, _totalDeaths);

            _asyncTimer.Start();
        }

        private async void OnAsyncTimerIntervalEnded(object sender, EventArgs e)
        {
            (KillsSinceReset, DeathsSinceReset, _totalKills, _totalDeaths) = await _killDeathService.GetKillsAndDeaths();
            KillsDeathsSummaryText = _killsDeathsSummaryTextService.UpdateAndReturnSummaryText(KillsSinceReset, _totalKills, _totalDeaths);
        }

        private void InitializeCommands()
        {
            ResetKillsAndDeathsCommand = new RelayCommand(ResetKillsAndDeaths);
            OpenSettingsCommand        = new RelayCommand(OpenSettingsWrapper);
            OnWindowLoadedCommand      = new RelayCommand(OnWindowLoaded);
            OnMouseEnterCommand        = new RelayCommand(() => TitleBarButtonsAreVisible = true);
            OnMouseLeaveCommand        = new RelayCommand(() => TitleBarButtonsAreVisible = false);
            OnWindowClosingCommand     = new RelayCommand(OnWindowClosing);
        }

        public RelayCommand OnWindowLoadedCommand { get; set; }
        public RelayCommand OnMouseEnterCommand { get; set; }
        public RelayCommand OnMouseLeaveCommand { get; set; }
        public RelayCommand ResetKillsAndDeathsCommand { get; set; }
        public RelayCommand OpenSettingsCommand { get; set; }
        public RelayCommand OnWindowClosingCommand { get; set; }

        //private const int API_REQUEST_INTERVAL_IN_SECONDS = 5 * 1; // todo for tests only. has to be commented in commit
        private const int API_REQUEST_INTERVAL_IN_SECONDS = 5 * 60;
        private readonly AsyncTimer _asyncTimer = new AsyncTimer(API_REQUEST_INTERVAL_IN_SECONDS);
        private readonly KillDeathService _killDeathService = new KillDeathService();
        private readonly KillsDeathsSummaryTextService _killsDeathsSummaryTextService = new KillsDeathsSummaryTextService();
        private int _killsSinceReset;
        private int _deathsSinceReset;
        private int _totalDeaths;
        private int _totalKills;
        private bool _resetButtonIsEnabled = true;
        private string _killsDeathsSummaryText = string.Empty;
        private bool _titleBarButtonsAreVisible;
    }
}