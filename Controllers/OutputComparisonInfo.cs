using System;
using System.Collections.Generic;
using System.Text;
using UserClasses;
using Controllers;
using System.Threading.Tasks;

namespace Controllers
{
    public class OutputComparisonInfo
    {
        static Dictionary<string, double> TimeOdd(double[] TimeSummariesFirst, double[] TimeSummariesSecond)
        {
            double distinction;
            Dictionary<string, double> timeOdd = new Dictionary<string, double>();
            if (TimeSummariesFirst[0] * 60 + TimeSummariesFirst[1] > TimeSummariesSecond[0] * 60 + TimeSummariesSecond[1]) distinction = (Math.Round(TimeSummariesFirst[0]) + Math.Round(TimeSummariesFirst[1]) / 100.0) - (Math.Round(TimeSummariesSecond[0]) + Math.Round(TimeSummariesSecond[1]) / 100.0);
            else distinction = (Math.Round(TimeSummariesSecond[0]) + Math.Round(TimeSummariesSecond[1]) / 100.0) - (Math.Round(TimeSummariesFirst[0]) + Math.Round(TimeSummariesFirst[1]) / 100.0);

            if (Math.Round((distinction - Math.Floor(distinction)) * 100 - 40) > 0)
            {
                timeOdd.Add("hours", Math.Round(Math.Floor(distinction)));
                timeOdd.Add("minutes", Math.Round((distinction - Math.Floor(distinction)) * 100 - 40));
            }
            else
            {
                timeOdd.Add("hours", Math.Round(Math.Floor(distinction)) - 1);
                timeOdd.Add("minutes", Math.Round((distinction - Math.Floor(distinction)) * 100 + 20));
            }
            return timeOdd;
        }

        public static void InputTimeComparison(PlayerComparison comparison, int time)
        {
            var TimeSummariesFirst = comparison.TimeComparison(comparison.FirstPlayerID);
            var TimeSummariesSecond = comparison.TimeComparison(comparison.SecondPlayerID);

            var getFirstPlayerInfo = new GetUserInfo(Deciphers.ConvertToSteamID64((int)TimeSummariesFirst[2]));
            getFirstPlayerInfo.DeterminatePlayerInfo();
            var getSecondPlayerInfo = new GetUserInfo(Deciphers.ConvertToSteamID64((int)TimeSummariesSecond[2]));
            getSecondPlayerInfo.DeterminatePlayerInfo();

            var Distinction = TimeOdd(TimeSummariesFirst, TimeSummariesSecond);

            if (TimeSummariesFirst[0] * 60 + TimeSummariesFirst[1] > TimeSummariesSecond[0] * 60 + TimeSummariesSecond[1])
            {
                Console.WriteLine($"{getFirstPlayerInfo.Login}: " + TimeSummariesFirst[0] + " hours,  " + Math.Round(TimeSummariesFirst[1]) + " minutes\n" +
                 getSecondPlayerInfo.Login + ": " + TimeSummariesSecond[0] + " hours, " + Math.Round(TimeSummariesSecond[1]) + " minutes\n" +
                 getFirstPlayerInfo.Login + " played " + Distinction["hours"] + " hours and " + Distinction["minutes"] + " minutes more, then the " + getSecondPlayerInfo.Login +
                " in " + time + " days");
            }
            else if (TimeSummariesFirst[0] * 60 + TimeSummariesFirst[1] < TimeSummariesSecond[0] * 60 + TimeSummariesSecond[1])
            {
                Console.Write($"{getFirstPlayerInfo.Login}: " + TimeSummariesFirst[0] + "  hours, " + Math.Round(TimeSummariesFirst[1]) + " minutes\n" +
                getSecondPlayerInfo.Login + ": " + TimeSummariesSecond[0] + " hours, " + Math.Round(TimeSummariesSecond[1]) + "  minutes\n" +
                getSecondPlayerInfo.Login + " played " + Distinction["hours"] + " hours and " + Distinction["minutes"] + " minutes more, then the " + getFirstPlayerInfo.Login +
                " in " + time + " days");
            }
            else
            {
                Console.Write($"Both Players played same amount of time in " + time + " days");
            }
        }

        public static void InputWinrateComparison(int time, decimal FirstSteamID, decimal SecondSteamID)
        {
            var getfirstplayerinfo = new GetUserInfo(Deciphers.ConvertToSteamID64(FirstSteamID));
            getfirstplayerinfo.DeterminatePlayerInfo();
            var getsecondplayerinfo = new GetUserInfo(Deciphers.ConvertToSteamID64(SecondSteamID));
            getsecondplayerinfo.DeterminatePlayerInfo();

            var comaprison = new PlayerComparison(FirstSteamID, SecondSteamID, time);
            var Stats = comaprison.WinRateAndRankComparison(comaprison.FirstPlayerID);
            

            var  OutputResult = $"{getfirstplayerinfo.Login} stats for " + time + " days: \n" +
                "Count mathes: " + Stats["Player matches"] + "\n" +
                "Wins: " + Stats["Player wins"] + "\n" +
                "Defeats: " + Stats["Player defeats"] + "\n" +
                "Winrate: " + Stats["Player winrate"] + "%\n" +
                "MMR: " + Stats["Player MMR"] + "\n";

            Stats = comaprison.WinRateAndRankComparison(comaprison.SecondPlayerID);
            OutputResult += $"{getsecondplayerinfo.Login} stats for " + time + " days: \n" +
                "Count mathes: " + Stats["Player matches"] + "\n" +
                "Wins: " + Stats["Player wins"] + "\n" +
                "Defeats: " + Stats["Player defeats"] + "\n" +
                "Winrate: " + Stats["Player winrate"] + "%\n" +
                "MMR: " + Stats["Player MMR"] + "\n";

            Console.WriteLine(OutputResult);
        }

        public async void OutputAverageHeroStatsInfo(decimal FirstSteamID, decimal SecondSteamID, string heroName)
        {
            var getherostats = new GetHeroStats();
            var getfirstplayerinfo = new GetUserInfo(Deciphers.ConvertToSteamID64(FirstSteamID));
            getfirstplayerinfo.DeterminatePlayerInfo();
            var getsecondplayerinfo = new GetUserInfo(Deciphers.ConvertToSteamID64(SecondSteamID));
            getsecondplayerinfo.DeterminatePlayerInfo();

        //  var averageHeroStats = (FirstPlayer: getherostats.GetAverageHeroResults(FirstSteamID, heroName), SecondPlayer: getherostats.GetAverageHeroResults(SecondSteamID, heroName));
            var tasks = new List<Task<Dictionary<string, double>>>();
            tasks.Add(getherostats.GetAverageHeroResultsAsync(FirstSteamID, heroName));
            tasks.Add(getherostats.GetAverageHeroResultsAsync(SecondSteamID, heroName));
            await Task.WhenAll(tasks);
            var averageHeroStats = (FirstPlayer: tasks[0].Result, SecondPlayer: tasks[1].Result);
          
            Console.WriteLine($"\t\tAverage {heroName} stats:\n" +
                $"\t\t{getfirstplayerinfo.Login}\t{getsecondplayerinfo.Login}\n" +
                $"KDA\t\t{averageHeroStats.FirstPlayer["KDA"]}\t\t{averageHeroStats.SecondPlayer["KDA"]}\n" +
                $"GPM\t\t{averageHeroStats.FirstPlayer["GPM"]}\t\t{averageHeroStats.SecondPlayer["GPM"]}\n" +
                $"EPM\t\t{averageHeroStats.FirstPlayer["EPM"]}\t\t{averageHeroStats.SecondPlayer["EPM"]}\n" +
                $"Hero damage\t{averageHeroStats.FirstPlayer["HeroDamage"]}\t\t{averageHeroStats.SecondPlayer["HeroDamage"]}\n" +
                $"NetWorth\t{averageHeroStats.FirstPlayer["NetWorth"]}\t\t{averageHeroStats.SecondPlayer["NetWorth"]}\n" +
                $"Winrate\t\t{averageHeroStats.FirstPlayer["WinRate"]}%\t\t{averageHeroStats.SecondPlayer["WinRate"]}%\n");

        }


    }
}
