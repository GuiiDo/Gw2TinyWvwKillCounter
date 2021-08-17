using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Gw2TinyWvwKillCounter.LogFile
{
    public class LogToFileConfigurationService
    {
        // experiment to replace the log4net config file and make log4net configuration more visible in code because this service has to be called somewhere

        public static void InitializeConfiguration()
        {
            var logEntryFormat = GetLogEntryFormat();
            var rollingFileAppender = GetActivatedRollingFileAppender(logEntryFormat);
            AddAppenderToRootLogger(rollingFileAppender);
        }


        private static void AddAppenderToRootLogger(RollingFileAppender rollingFileAppender)
        {
            var loggerHierarchy = (Hierarchy)LogManager.GetRepository();
            loggerHierarchy.Root.AddAppender(rollingFileAppender);
            loggerHierarchy.Root.Level = Level.All;
            loggerHierarchy.Configured = true;
        }


        private static PatternLayout GetLogEntryFormat()
        {
            var patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [viewer] [%thread] %level %stacktrace - %message%newline%exception";
            patternLayout.ActivateOptions();

            return patternLayout;
        }


        private static RollingFileAppender GetActivatedRollingFileAppender(PatternLayout logEntryFormat)
        {



            var rollingFileAppender = new RollingFileAppender
            {
                Layout = logEntryFormat,
                File = LogToFilePathService.CreateLog4NetLogFilePath(), // because $(LOCALAPPDATA)\eLogViewer.log doesnt work, though it should.
                StaticLogFileName = true,
                AppendToFile = true,
                MaximumFileSize = "2MB",
                RollingStyle = RollingFileAppender.RollingMode.Size,
                MaxSizeRollBackups = 1  // better than 0, because with 0 the log file will be truncated everytime new entries are added -> overhead
            };

            rollingFileAppender.ActivateOptions();

            return rollingFileAppender;
        }
    }
}