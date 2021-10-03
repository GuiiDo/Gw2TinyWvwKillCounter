using System.Collections.ObjectModel;
using System.Linq;
using Gw2TinyWvwKillCounter.UiCommon;

namespace Gw2TinyWvwKillCounter
{
    public class UiSettings : PropertyChangedBase
    {
        public void BackupUiSettings()
        {
            _backupUiScalingInPercent = ScalingInPercent;
            _backupUiOpacityInPercent = OpacityInPercent;
        }

        public void RestoreUiSettings()
        {
            ScalingInPercent = _backupUiScalingInPercent;
            OpacityInPercent = _backupUiOpacityInPercent;
        }

        public ObservableCollection<string> UiScalingInPercentSelectableValues { get; set; } = UiSettings.CreateValuesFrom50To500InStepsOf10();
        public ObservableCollection<string> UiOpacityInPercentSelectableValues { get; set; } = UiSettings.CreateValuesFrom0To100InStepsOf10();

        public string UiScalingInPercent
        {
            get => ScalingInPercent.ToString();
            set => ScalingInPercent = uint.Parse(value);
        }

        public string UiOpacityInPercent
        {
            get => OpacityInPercent.ToString();
            set => OpacityInPercent = uint.Parse(value);
        }

        public uint OpacityInPercent
        {
            get => Properties.Settings.Default.OpacityInPercent;
            set
            {
                Properties.Settings.Default.OpacityInPercent = value;
                Properties.Settings.Default.Save();

                RaisePropertyChanged(nameof(Opacity));
            }
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

        public static ObservableCollection<string> CreateValuesFrom0To100InStepsOf10()
        {
            var values = Enumerable.Range(0, 11)
                                   .Select(i => i * 10)
                                   .Select(i => i.ToString());

            return new ObservableCollection<string>(values);
        }

        public double Opacity => (double) OpacityInPercent / 100 + 0.01; // + 0.01 to prevent full transparency because full transparency causes area to not

        // count as part of the window anymore and thus does not react to mouseEnter event anymore
        // then title bar buttons cannot be clicked anymore
        public double TooltipScaling => (double) ScalingInPercent / 100;
        public uint MainWindowWidth => DEFAULT_WINDOW_WIDTH * ScalingInPercent / 100;

        public uint MainWindowHeight => DEFAULT_WINDOW_HEIGHT * ScalingInPercent / 100; // no integer rounding problem because width/height are always multiple of 100.
        // when default width/height is changed and/or scalingInPercent has lesser step width
        // this can become an issue.

        private const uint DEFAULT_WINDOW_WIDTH = 60;
        private const uint DEFAULT_WINDOW_HEIGHT = 50;
        private uint _backupUiScalingInPercent;
        private uint _backupUiOpacityInPercent;
    }
}