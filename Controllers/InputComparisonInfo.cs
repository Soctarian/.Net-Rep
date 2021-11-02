using System;
using System.Collections.Generic;
using System.Text;

namespace Controllers
{
    public class InputComparisonInfo
    {
        static Dictionary<string, double> TimeOdd(double[] TimeSummaries)
        {
            double distinction;
            Dictionary<string, double> timeOdd = new Dictionary<string, double>();
            if (TimeSummaries[0] * 60 + TimeSummaries[1] > TimeSummaries[2] * 60 + TimeSummaries[3]) distinction = (TimeSummaries[0] + TimeSummaries[1] / 100.0) - (TimeSummaries[2] + TimeSummaries[3] / 100.0);
            else distinction = (TimeSummaries[2] + TimeSummaries[3] / 100.0) - (TimeSummaries[0] + TimeSummaries[1] / 100.0);

            timeOdd.Add("hours", Math.Floor(distinction));
            timeOdd.Add("minutes", Math.Round((distinction - Math.Floor(distinction)) * 100 - 40));
            return timeOdd;
        }
        public static void InputTimeComparison(double[] TimeSummaries, int time)
        {
            var Distinction = TimeOdd(TimeSummaries);

            if (TimeSummaries[0] * 60 + TimeSummaries[1] > TimeSummaries[2] * 60 + TimeSummaries[3])
            {
                Console.Write($"First Player: " + TimeSummaries[0] + " hours,  " + Math.Round(TimeSummaries[1]) + " minutes\n" +
                "Second Player: " + TimeSummaries[2] + " hours, " + Math.Round(TimeSummaries[3]) + " minutes\n" +
                "First player played " + Distinction["hours"] + " hours and " + Distinction["minutes"] + " minutes more, then the Second player" +
                "in " + time + " days");
            }
            else if (TimeSummaries[0] * 60 + TimeSummaries[1] < TimeSummaries[2] * 60 + TimeSummaries[3])
            {
                Console.Write($"First Player: " + TimeSummaries[0] + "  hours, " + Math.Round(TimeSummaries[1]) + " minutes\n" +
                "Second Player: " + TimeSummaries[2] + " hours, " + Math.Round(TimeSummaries[3]) + "  minutes\n" +
                "Second player played " + Distinction["hours"] + " hours and " + Distinction["minutes"] + " minutes more, then the First player" +
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
