using System.Collections.ObjectModel;
using System.Linq;
using Gw2TinyWvwKillCounter.UiCommon;

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

                RaisePropertyChanged(nameof(TooltipScaling));
                RaisePropertyChanged(nameof(MainWindowWidth));
                RaisePropertyChanged(nameof(MainWindowHeight));
            }
        }

        public static ObservableCollection<string> CreateValuesFrom50To500InStepsOf10()
        {
            var values = Enumerable.Range(5, 46)
                                   .Select(i => i * 10)
                                   .Select(i => i.ToString());

            return new ObservableCollection<string>(values);
        }

        public double TooltipScaling => (double)ScalingInPercent / 100;
        public uint MainWindowWidth => DEFAULT_WINDOW_WIDTH * ScalingInPercent / 100;
        public uint MainWindowHeight => DEFAULT_WINDOW_HEIGHT * ScalingInPercent / 100; // no integer rounding problem because width/height are always multiple of 100.
                                                                                        // when default width/height is changed and/or scalingInPercent has lesser step width
                                                                                        // this can become an issue.

        private const uint DEFAULT_WINDOW_WIDTH = 60;
        private const uint DEFAULT_WINDOW_HEIGHT = 50;
        private uint _backupUiScalingInPercent;
    }
}