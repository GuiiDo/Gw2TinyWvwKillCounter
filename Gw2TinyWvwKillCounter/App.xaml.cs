using System.Windows;
using System.Windows.Controls;

namespace Gw2TinyWvwKillCounter
{
    public partial class App : Application
    {
        public App()
        {
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(UIElement), new FrameworkPropertyMetadata(60 * 1000));
            ToolTipService.ShowOnDisabledProperty.OverrideMetadata(typeof(UIElement), new FrameworkPropertyMetadata(true));
        }
    }
}
