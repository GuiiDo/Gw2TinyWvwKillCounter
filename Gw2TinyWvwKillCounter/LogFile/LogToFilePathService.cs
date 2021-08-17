using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace Gw2TinyWvwKillCounter.LogFile
{
    public class LogToFilePathService
    {
        public static string CreateLog4NetLogFilePath()
        {
            try
            {
                return CreatePathToAppSettingsFolder();
            }
            catch (Exception)
            {
                return CreatePathToExecutableFolder();
            }
        }


        private static string CreatePathToExecutableFolder()
        {
            var appName = GetAssemblyName();
            return $"{appName}.log";
        }


        private static string CreatePathToAppSettingsFolder()
        {
            var localAppDataPath      = GetLocalAppDataPath();
            var appSettingsFolderName = GetAppSettingsFolderName();
            var appName               = GetAssemblyName();
            return Path.Combine(localAppDataPath, appSettingsFolderName, $"{appName}.log");
        }


        private static string GetAssemblyName()
        {
            return Assembly.GetExecutingAssembly()
                           .GetName()
                           .Name;
        }


        private static string GetAppSettingsFolderName()
        {
            return typeof(App).Namespace;
        }


        private static string GetLocalAppDataPath()
        {
            foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables())
            {
                var environmentVariableString = environmentVariable.Key as string;
                if (environmentVariableString?.ToLower() == "localappdata")
                    return (string) environmentVariable.Value;
            }

            return string.Empty;
        }
    }
}