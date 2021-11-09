using System;
using System.Collections.Generic;
using System.Text;
using UserClasses;
using Controllers;


namespace Controllers
{
    public class InputComparisonInfo
    {
        static Dictionary<string, double> TimeOdd(double[] TimeSummaries)
        {
            double distinction;
            Dictionary<string, double> timeOdd = new Dictionary<string, double>();
            if (TimeSummaries[0] * 60 + TimeSummaries[1] > TimeSummaries[2] * 60 + TimeSummaries[3]) distinction = (Math.Round(TimeSummaries[0]) + Math.Round(TimeSummaries[1]) / 100.0) - (Math.Round(TimeSummaries[2]) + Math.Round(TimeSummaries[3]) / 100.0);
            else distinction = (Math.Round(TimeSummaries[2]) + Math.Round(TimeSummaries[3]) / 100.0) - (Math.Round(TimeSummaries[0]) + Math.Round(TimeSummaries[1]) / 100.0);

            if(Math.Round((distinction - Math.Floor(distinction)) * 100 - 40)>0)
            {
                timeOdd.Add("hours", Math.Round(Math.Floor(distinction)));
                timeOdd.Add("minutes", Math.Round((distinction - Math.Floor(distinction)) * 100 - 40));
            }
            else
            {
                timeOdd.Add("hours", Math.Round(Math.Floor(distinction))-1);
                timeOdd.Add("minutes", Math.Round((distinction - Math.Floor(distinction)) * 100 + 20));
            }
            return timeOdd;
        }
        public static void InputTimeComparison(double[] TimeSummaries, int time)
        {
            var getFirstPlayerInfo = new GetUserInfo();
            getFirstPlayerInfo.DeterminatePlayerInfo(Deciphers.ConvertToSteamID64(Convert.ToDecimal(TimeSummaries[4])));
            var getSecondPlayerInfo = new GetUserInfo();
            getSecondPlayerInfo.DeterminatePlayerInfo(Deciphers.ConvertToSteamID64(Convert.ToDecimal(TimeSummaries[5])));

            var Distinction = TimeOdd(TimeSummaries);
            
            if (TimeSummaries[0] * 60 + TimeSummaries[1] > TimeSummaries[2] * 60 + TimeSummaries[3])
            {
                Console.Write($"{getFirstPlayerInfo.Login}: " + TimeSummaries[0] + " hours,  " + Math.Round(TimeSummaries[1]) + " minutes\n" +
                 getSecondPlayerInfo.Login + ": " + TimeSummaries[2] + " hours, " + Math.Round(TimeSummaries[3]) + " minutes\n" +
                 getFirstPlayerInfo.Login + " played " + Distinction["hours"] + " hours and " + Distinction["minutes"] + " minutes more, then the "+ getSecondPlayerInfo.Login +
                "in " + time + " days");
            }
            else if (TimeSummaries[0] * 60 + TimeSummaries[1] < TimeSummaries[2] * 60 + TimeSummaries[3])
            {
                Console.Write($"{getFirstPlayerInfo.Login}: " + TimeSummaries[0] + "  hours, " + Math.Round(TimeSummaries[1]) + " minutes\n" +
                getSecondPlayerInfo.Login + ": " + TimeSummaries[2] + " hours, " + Math.Round(TimeSummaries[3]) + "  minutes\n" +
                getSecondPlayerInfo.Login + " played " + Distinction["hours"] + " hours and " + Distinction["minutes"] + " minutes more, then the "+ getFirstPlayerInfo.Login +
                "in " + time + " days");
            }
            else
            {
                Console.Write($"Both Players played same amount of time in " + time + " days");
            }
        }

        public static void InputWinrateComparison(Dictionary<string, int> Stats, int time)
        {
            Console.WriteLine($"First player stats for " + time + " days: \n" +
                "Count mathes: " + Stats["First player matches"] + "\n" +
                "Wins: " + Stats["First player wins"] + "\n" +
                "Defeats: " + Stats["First player defeats"] + "\n" +
                "Winrate: " + Stats["First player winrate"] + "%\n" +
                "Second player stats for " + time + " days: \n" +
                "Count mathes: " + Stats["Second player matches"] + "\n" +
                "Wins: " + Stats["Second player wins"] + "\n" +
                "Defeats: " + Stats["Second player defeats"] + "\n" +
                "Winrate: " + Stats["Second player winrate"] + "%\n");

        }



    }
}
