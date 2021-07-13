using System;
using System.Linq;
using System.Threading.Tasks;
using Gw2Sharp;

namespace Gw2TinyWvwKillCounter.Services
{
    public class KillDeathService
    {
        public async Task<(int totalKillsAtReset, int totalDeathsAtReset)> InitialiseAndGetTotalKillsDeath(string apiKey)
        {
            _gw2Client = new Gw2Client(new Connection(apiKey));

            (_totalKills, _totalDeaths) = await GetTotalKillsAndDeaths(_gw2Client);
            ResetKillsAndDeaths();

            return (_totalKills, _totalDeaths);
        }

        public void ResetKillsAndDeaths()
        {
            _totalKillsAtReset = _totalKills;
            _totalDeathsAtReset = _totalDeaths;
        }

        public async Task<(int killsSinceReset, int deathsSinceReset, int totalKills, int totalDeaths)> GetKillsAndDeathsSinceReset()
        {
            (_totalKills, _totalDeaths) = await GetTotalKillsAndDeaths(_gw2Client);

            var killsSinceReset  = _totalKills - _totalKillsAtReset;
            var deathsSinceReset = _totalDeaths - _totalDeathsAtReset;
            return (killsSinceReset, deathsSinceReset, _totalKills, _totalDeaths);
        }

        private async Task<(int totalKills, int totalDeaths)> GetTotalKillsAndDeaths(Gw2Client gw2Client)
        {
            //var characterTask    = gw2Client.WebApi.V2.Characters[CHARACTER_NAME].GetAsync();
            var charactersTask   = gw2Client.WebApi.V2.Characters.AllAsync();
            var achievementsTask = gw2Client.WebApi.V2.Account.Achievements.GetAsync();
            var accountTask      = gw2Client.WebApi.V2.Account.GetAsync(); // todo weg

            //var character    = await characterTask;
            await Task.WhenAll(charactersTask, achievementsTask, accountTask);
            var characters   = charactersTask.Result;
            var achievements = achievementsTask.Result;
            var account      = accountTask.Result; // todo weg

            Test = $"{DateTime.Now:HH:mm} now\n" + // todo ganz weg
                   $"{characters.HttpResponseInfo.Date.ToLocalTime().DateTime:HH:mm} date\n" +
                   $"{characters.HttpResponseInfo.Expires.Value.ToLocalTime().DateTime:HH:mm} expire\n" +
                   $"{account.HttpResponseInfo.LastModified.Value.ToLocalTime().DateTime:HH:mm} account last-modified";

            var totalDeaths = characters.Sum(c => c.Deaths);
            var totalKills  = achievements.Single(a => a.Id == REALM_AVENGER_ACHIEVEMENT_ID).Current;

            return (totalKills, totalDeaths);
        }

        public string Test { get; set; } // todo weg

        private int _totalKills;
        private int _totalDeaths;
        private int _totalKillsAtReset;
        private int _totalDeathsAtReset;
        private Gw2Client _gw2Client;
        private const int REALM_AVENGER_ACHIEVEMENT_ID = 283;
    }
}