using System.Windows;
using System.Windows.Controls;
using Gw2TinyWvwKillCounter.LogFile;

namespace Gw2TinyWvwKillCounter
{
    public partial class App : Application
    {
        public App()
        {
            LogToFileConfigurationService.InitializeConfiguration();
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(UIElement), new FrameworkPropertyMetadata(60 * 1000));
            ToolTipService.ShowOnDisabledProperty.OverrideMetadata(typeof(UIElement), new FrameworkPropertyMetadata(true));
        }
    }
}
