using System;
using System.Reflection;
using log4net.Core;

namespace Gw2TinyWvwKillCounter.LogFile
{
    public static class LogToFile
    {
        // cannot use ILog with Log.Info(..) etc. because %stacktrace would always return this log4net wrapper as caller instead of the method calling the LogToFile.Info(..) etc method.
        // https://stackoverflow.com/a/157891/4394435
        private static readonly ILogger Logger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), "MyLogger");

        // fatal = crash
        // error = no crash, but operation was probably canceled or result is not what user may expect -> user will be informed
        // info = user interaction and stuff
        public static void Fatal(string message) => Logger.Log(typeof(LogToFile), Level.Fatal, message, null);
        public static void Error(string message) => Logger.Log(typeof(LogToFile), Level.Error, message, null);
        public static void Info(string message) => Logger.Log(typeof(LogToFile), Level.Info, message, null);
        public static void Fatal(string message, Exception exception) => Logger.Log(typeof(LogToFile), Level.Fatal, message, exception);
        public static void Error(string message, Exception exception) => Logger.Log(typeof(LogToFile), Level.Error, message, exception);
        public static void Info(string message, Exception exception) => Logger.Log(typeof(LogToFile), Level.Info, message, exception);
    }
}