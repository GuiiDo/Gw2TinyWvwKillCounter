using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Gw2TinyWvwKillCounter
{
    public static class ShowDialogWithUnhandledExceptionService
    {
        public static void InitializeExceptionHandling()
        {
            TaskScheduler.UnobservedTaskException            += ShowUnobservedTaskExceptionAfterGarbageCollection;
            Application.Current.DispatcherUnhandledException += ShowUiThreadException;
            AppDomain.CurrentDomain.UnhandledException       += ShowAllThreadException;
        }

        private static void ShowUnobservedTaskExceptionAfterGarbageCollection(object sender, UnobservedTaskExceptionEventArgs e)
        {
            PreventExceptionEscalationToHigherExceptionHandler(e);
            MessageBox.Show(e.Exception.ToString(), "Sorry, Gw2TinyWvwKillCounter crashed :(");
            CloseAppWithoutSavingSettingsAndWithoutWindowsExceptionDialog();
        }

        private static void ShowUiThreadException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            PreventExceptionEscalationToHigherExceptionHandler(e);
            MessageBox.Show(e.Exception.ToString(), "Sorry, Gw2TinyWvwKillCounter crashed :(");
            CloseAppWithoutSavingSettingsAndWithoutWindowsExceptionDialog();
        }

        private static void ShowAllThreadException(object sender, UnhandledExceptionEventArgs e)
        {
            var unhandledException = e.ExceptionObject.ToString();
            MessageBox.Show(unhandledException, "Sorry, Gw2TinyWvwKillCounter crashed :(");
            CloseAppWithoutSavingSettingsAndWithoutWindowsExceptionDialog();
        }

        private static void PreventExceptionEscalationToHigherExceptionHandler(UnobservedTaskExceptionEventArgs e) => e.SetObserved();
        private static void PreventExceptionEscalationToHigherExceptionHandler(DispatcherUnhandledExceptionEventArgs e) => e.Handled = true;

        // NOT Application.Current.Shutdown(), because this calls save settings to file like a normal shut down (tested) and this way may save corrupt settings.
        private static void CloseAppWithoutSavingSettingsAndWithoutWindowsExceptionDialog() 
            => Application.Current.Dispatcher.Invoke(() => Process.GetCurrentProcess().Kill());
    }
}