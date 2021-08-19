using System;
using System.Linq;
using System.Threading.Tasks;
using Gw2Sharp;
using Gw2Sharp.WebApi.Exceptions;
using Gw2TinyWvwKillCounter.LogFile;
using Gw2Sharp.WebApi.Http;

namespace Gw2TinyWvwKillCounter.Api
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

        public async Task<(int killsSinceReset, int deathsSinceReset, int totalKills, int totalDeaths)> GetKillsAndDeaths()
        {
            try
            {
                (_totalKills, _totalDeaths) = await GetTotalKillsAndDeaths(_gw2Client);

                //var fakeWebApiRequest = new WebApiRequest(new Uri("https://www.google.com/"), new Connection(), new Gw2Client()); // todo weg
                //throw new NotFoundException(fakeWebApiRequest, new WebApiResponse<ErrorObject>(new ErrorObject(), new CacheState())); // todo weg
                //throw new UnexpectedStatusException(fakeWebApiRequest, new WebApiResponse("", new CacheState())); // todo weg
            }
            catch (Exception e)
            {
                // intentionally no error handling!
                // when api server does not respond (error code 500, 502) or times out (RequestCanceledException)
                LogToFile.Error("GetKillsAndDeaths crash", e);
                // the app will just return the previous kill/death values and hope that on the end of the next interval
                // the api server will answer correctly again.
                // todo Problem: this catches every exception from gw2sharp. so this may hide crashes that should be caught and handled in a different way.
                // todo e.g. TooManyRequestsException (when other tools use the same api key?), BadRequestException (api key deleted on gw2 website?),
                // todo AuthorizationRequiredException (something is wrong with api key etc.)
            }

            var killsSinceReset  = _totalKills - _totalKillsAtReset;
            var deathsSinceReset = _totalDeaths - _totalDeathsAtReset;
            return (killsSinceReset, deathsSinceReset, _totalKills, _totalDeaths);
        }

        private static async Task<(int totalKills, int totalDeaths)> GetTotalKillsAndDeaths(Gw2Client gw2Client)
        {
            var charactersTask   = gw2Client.WebApi.V2.Characters.AllAsync();
            var achievementsTask = gw2Client.WebApi.V2.Account.Achievements.GetAsync();
            var wvwTask          = gw2Client.WebApi.V2.Wvw.Matches.Stats.GetAsync("2-4"); // todo weg

            await Task.WhenAll(charactersTask, achievementsTask, wvwTask);
            var characters   = charactersTask.Result;
            var achievements = achievementsTask.Result;
            var wvw          = wvwTask.Result; // todo weg
            var totalDeaths  = characters.Sum(c => c.Deaths);
            var totalKills   = achievements.Single(a => a.Id == REALM_AVENGER_ACHIEVEMENT_ID).Current;

            try // todo weg
            {
                Test = $"{DateTime.Now:HH:mm:ss}\n" +
                       $"red\n" +
                       $"t   {wvw.Kills.Red} {wvw.Deaths.Red} {(float) wvw.Kills.Red / wvw.Deaths.Red:0.00}\n" + // todo weg
                       $"ebg {wvw.Maps[0].Kills.Red} {wvw.Maps[0].Deaths.Red} {(float) wvw.Maps[0].Kills.Red / wvw.Maps[0].Deaths.Red:0.00} \n" +
                       $"rbl {wvw.Maps[1].Kills.Red} {wvw.Maps[1].Deaths.Red} {(float) wvw.Maps[1].Kills.Red / wvw.Maps[1].Deaths.Red:0.00} \n" +
                       $"bbl {wvw.Maps[2].Kills.Red} {wvw.Maps[2].Deaths.Red} {(float) wvw.Maps[2].Kills.Red / wvw.Maps[2].Deaths.Red:0.00} \n" +
                       $"gbl {wvw.Maps[3].Kills.Red} {wvw.Maps[3].Deaths.Red} {(float) wvw.Maps[3].Kills.Red / wvw.Maps[3].Deaths.Red:0.00} \n\n" +
                       $"blue\n" +
                       $"t   {wvw.Kills.Blue} {wvw.Deaths.Blue} {(float) wvw.Kills.Blue / wvw.Deaths.Blue:0.00} \n" +
                       $"ebg {wvw.Maps[0].Kills.Blue} {wvw.Maps[0].Deaths.Blue} {(float) wvw.Maps[0].Kills.Blue / wvw.Maps[0].Deaths.Blue:0.00} \n" +
                       $"rbl {wvw.Maps[1].Kills.Blue} {wvw.Maps[1].Deaths.Blue} {(float) wvw.Maps[1].Kills.Blue / wvw.Maps[1].Deaths.Blue:0.00} \n" +
                       $"bbl {wvw.Maps[2].Kills.Blue} {wvw.Maps[2].Deaths.Blue} {(float) wvw.Maps[2].Kills.Blue / wvw.Maps[2].Deaths.Blue:0.00} \n" +
                       $"gbl {wvw.Maps[3].Kills.Blue} {wvw.Maps[3].Deaths.Blue} {(float) wvw.Maps[3].Kills.Blue / wvw.Maps[3].Deaths.Blue:0.00} \n\n" +
                       $"green\n" +
                       $"t   {wvw.Kills.Green} {wvw.Deaths.Green} {(float) wvw.Kills.Green / wvw.Deaths.Green:0.00} \n" +
                       $"ebg {wvw.Maps[0].Kills.Green} {wvw.Maps[0].Deaths.Green} {(float) wvw.Maps[0].Kills.Green / wvw.Maps[0].Deaths.Green:0.00} \n" +
                       $"rbl {wvw.Maps[1].Kills.Green} {wvw.Maps[1].Deaths.Green} {(float) wvw.Maps[1].Kills.Green / wvw.Maps[1].Deaths.Green:0.00} \n" +
                       $"bbl {wvw.Maps[2].Kills.Green} {wvw.Maps[2].Deaths.Green} {(float) wvw.Maps[2].Kills.Green / wvw.Maps[2].Deaths.Green:0.00} \n" +
                       $"gbl {wvw.Maps[3].Kills.Green} {wvw.Maps[3].Deaths.Green} {(float) wvw.Maps[3].Kills.Green / wvw.Maps[3].Deaths.Green:0.00} \n ";

                LogToFile.Info(Test);

            }
            catch (Exception)
            {
                // catch divide zero. too lazy to write code for that now. 
            }

            return (totalKills, totalDeaths);
        }

        public static string Test { get; set; } = string.Empty; // todo weg

        private int _totalKills;
        private int _totalDeaths;
        private int _totalKillsAtReset;
        private int _totalDeathsAtReset;
        private Gw2Client _gw2Client;
        private const int REALM_AVENGER_ACHIEVEMENT_ID = 283;
    }
}