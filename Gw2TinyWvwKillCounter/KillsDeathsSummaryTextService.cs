using System;
using System.Collections.Generic;
using System.Linq;

namespace Gw2TinyWvwKillCounter
{
    public class KillsDeathsSummaryTextService
    {
        public string ResetAndReturnSummaryText(int totalKills, int totalDeaths)
        {
            _startTime = DateTime.Now;
            _historyEntries.Clear();
            InsertNewHistoryEntryAtBeginning(0, _historyEntries);

            var sessionDuration     = new TimeSpan(0);
            var sessionKillsPerHour = 0;
            return CreateSummaryText(totalKills, totalDeaths, sessionKillsPerHour, sessionDuration, _historyEntries);
        }

        public string UpdateAndReturnSummaryText(int killsSinceReset, int totalKills, int totalDeaths)
        {
            InsertNewHistoryEntryAtBeginning(killsSinceReset, _historyEntries);

            if (HistoryIsTooLong(_historyEntries.Count))
                RemoveOldestHistoryEntry(_historyEntries);

            var sessionDuration     = DateTime.Now - _startTime;
            var sessionKillsPerHour = killsSinceReset / sessionDuration.TotalHours;
            return CreateSummaryText(totalKills, totalDeaths, sessionKillsPerHour, sessionDuration, _historyEntries);
        }

        private string CreateSummaryText(int totalKills, int totalDeaths, double sessionKillsPerHour, TimeSpan sessionDuration, List<string> historyEntries)
        {
            return $"{totalKills} kills (total)\n" +
                   $"{totalDeaths} deaths (total, all characters)\n" +
                   $"{sessionKillsPerHour:0} kills/hour (session)\n" +
                   $"{sessionDuration:hh':'mm} hour : minute (session)\n\n" +
                   $"Time  | kills (session)\n" +
                   $"{string.Join("\n", historyEntries)}";
        }

        private static void InsertNewHistoryEntryAtBeginning(int kills, List<string> historyEntries)
        {
            historyEntries.Insert(0, $"{DateTime.Now:HH:mm} | {kills}");
        }

        private static bool HistoryIsTooLong(int entriesCount)
        {
            return entriesCount > MAX_NUMBER_OF_ENTRIES;
        }

        private static void RemoveOldestHistoryEntry(List<string> historyEntries)
        {
            historyEntries.Remove(historyEntries.Last());
        }

        private DateTime _startTime;
        private readonly List<string> _historyEntries = new List<string>();
        private const int MAX_NUMBER_OF_ENTRIES = 12;
    }
}