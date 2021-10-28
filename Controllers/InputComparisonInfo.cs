using System;
using System.Collections.Generic;
using System.Text;

namespace Controllers
{
    public class InputComparisonInfo
    {
        public static void InputTimeComparison(double[] TimeSummaries, int time)
        {
            //Подкорректировать вывод разницы + нормально округлить минуты + подкорректировать проверку на большее время мем
            if (TimeSummaries[0] + TimeSummaries[1] > TimeSummaries[2] + TimeSummaries[3])
            {
                Console.Write($"First Player: hours - " + TimeSummaries[0] + " minutes - " + TimeSummaries[1] + "\n" +
                "Second Player: hours - " + TimeSummaries[2] + " minutes - " + TimeSummaries[3] +
                "First player played " + (TimeSummaries[0] - TimeSummaries[2]) + " hours and " + Math.Round((TimeSummaries[1] - TimeSummaries[3])) + " minutes more,\nthen the Second player" +
                "in " + time + " days");
            }
            else if (TimeSummaries[0] + TimeSummaries[1] < TimeSummaries[2] + TimeSummaries[3])
            {
                Console.Write($"First Player: hours - " + TimeSummaries[0] + " minutes - " + TimeSummaries[1] + "\n" +
                "Second Player: hours - " + TimeSummaries[2] + " minutes - " + TimeSummaries[3] +
                "Second player played " + (TimeSummaries[2] - TimeSummaries[0]) + " hours and " + Math.Round((TimeSummaries[3] - TimeSummaries[1])) + " minutes more,\nthen the First player" +
                "in " + time + " days");
            }
            else
            {
                Console.Write($"Both Players played same amount of time in " + time + " days");
            }
        }
    }
}
