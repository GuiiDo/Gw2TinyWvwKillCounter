using System.Collections.Generic;
using Gw2TinyWvwKillCounter.Properties;
using Newtonsoft.Json;

namespace Gw2TinyWvwKillCounter.Api
{
    public class ApiKeyService
    {
        public static bool PersistedSelectedApiKeyExists => Settings.Default.SelectedApiKey != string.Empty;

        public static ApiKey PersistedSelectedApiKey
        {
            get => JsonConvert.DeserializeObject<ApiKey>(Settings.Default.SelectedApiKey) ?? new ApiKey();
            set
            {
                Settings.Default.SelectedApiKey = value == null 
                    ? new ApiKey().ToJson() 
                    : value.ToJson();

                Settings.Default.Save();
            }
        }

        public static List<ApiKey> PersistedApiKeys
        {
            get => JsonConvert.DeserializeObject<List<ApiKey>>(Settings.Default.ApiKeys) ?? new List<ApiKey>();
            set
            {
                Settings.Default.ApiKeys = JsonConvert.SerializeObject(value, Formatting.None);
                Settings.Default.Save();
            }
        }
    }
}