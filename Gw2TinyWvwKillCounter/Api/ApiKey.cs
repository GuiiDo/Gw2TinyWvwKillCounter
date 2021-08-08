using System;
using Newtonsoft.Json;

namespace Gw2TinyWvwKillCounter.Api
{
    public class ApiKey
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty; // that is the key. not called key because it is easy to mix up with "ApiKey" object itself.
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }

        public override bool Equals(object apiKeyAsObject) // todo weg?? weil json vergleich besser lesbar
        {
            return apiKeyAsObject switch
            {
                null          => false,
                ApiKey apiKey => Name == apiKey.Name && Value == apiKey.Value,
                _             => false
            };
        }

        public override int GetHashCode() // implemented to suppress visual studio warning that it is missing.
        {
            return HashCode.Combine(Name, Value);
        }
    }
}