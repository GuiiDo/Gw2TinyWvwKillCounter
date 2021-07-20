using Gw2TinyWvwKillCounter.DefaultUiStuff;

namespace Gw2TinyWvwKillCounter
{
    public class UiScaling : PropertyChangedBase
    {
        public void BackupUiScaling()
        {
            _backupUiScalingInPercent = ScalingInPercent;
        }

        public void RestoreUiScaling()
        {
            ScalingInPercent = _backupUiScalingInPercent;
        }


        public uint ScalingInPercent
        {
            get => Properties.Settings.Default.ScalingInPercent;
            set
            {
                Properties.Settings.Default.ScalingInPercent = value;
                Properties.Settings.Default.Save();

                RaisePropertyChanged(nameof(MainWindowWidth));
                RaisePropertyChanged(nameof(MainWindowHeight));
            }
        }

        public uint MainWindowWidth => DEFAULT_WINDOW_WIDTH * ScalingInPercent / 100;
        public uint MainWindowHeight => DEFAULT_WINDOW_HEIGHT * ScalingInPercent / 100;

        private const uint DEFAULT_WINDOW_WIDTH = 60;
        private const uint DEFAULT_WINDOW_HEIGHT = 50;
        private uint _backupUiScalingInPercent;
    }
}