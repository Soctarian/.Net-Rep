using System;
using System.Collections.Generic;
using System.Text;
using DeserializeObjects;
using System.Linq;
namespace Controllers
{
    public class PlayerComparison
    {
        
      /*                              План:
        Спарсить все матчи
        Отобрать айди тех, которые происходили за неделю\месяц\год\n-количество дней
        Спарсить все детали этих матчей и суммировать продолжительности игр
        Вывести время для 1 и 2 игра, провести необходимые сравнения*/

        public static List<decimal> GetMatchesArray(GetMatchHistory.Root Player, int time)
        {
            var matches = Player.result.Matches
                .Where(player => Deciphers.UnixTimeStampToDateTime(player.start_time) < DateTime.Now
                && Deciphers.UnixTimeStampToDateTime(player.start_time) > DateTime.Now.AddDays(-time));
            List<decimal> matchIDs = new List<decimal>();
            foreach (var match in matches)
            {
                matchIDs.Add(match.MatchId);
            }
            return matchIDs;
        }

        public void TimeComparison(decimal FirstPlayerID, decimal SecondPlayerID, int time)
        {
            var Players = (GetUrls.GetMatchHistoryUrl(FirstPlayerID), GetUrls.GetMatchHistoryUrl(SecondPlayerID));
            var MatchIDs = (GetMatchesArray(Players.Item1, time), GetMatchesArray(Players.Item2, time));
            List<GetMatchDetails.Root> FirstListMatches = new List<GetMatchDetails.Root>();
            List<GetMatchDetails.Root> SecondListMatches = new List<GetMatchDetails.Root>();
            //0 - hours, 1 - minutes for First Player, 2 - hours, 3 - minutes for Second Player
            double[] TimeSummaries = { 0, 0, 0, 0 };
            foreach (decimal ID in MatchIDs.Item1) FirstListMatches.Add(GetUrls.GetMatchDetailsUrl(ID));
            foreach (decimal ID in MatchIDs.Item2) SecondListMatches.Add(GetUrls.GetMatchDetailsUrl(ID));
            foreach (var match in FirstListMatches)
            {
                /*Время приходит в секундах, при конвертации в минуты => 2254/60=37.56 получаем дробное значение, которое неправильно отображает время,
                так как 37.5666667 ~ 37 + (0.5666667*60)/100 = 37.34 (37 минут, 34 секунды), данные в массивы заносятся именно в таком формате*/
                TimeSummaries[0] += match.result.duration / 3600.0 > 1 ? match.result.duration / 3600.0 : 0;
                TimeSummaries[1] += match.result.duration / 3600.0 > 1 ? Math.Truncate(match.result.duration / 3600.0) * 60 : (match.result.duration / 60) + ((match.result.duration / 60.0 - Math.Truncate(match.result.duration / 60.0)) * 60) / 100.0;


            }
            foreach (var match in SecondListMatches)
            {
                TimeSummaries[2] += match.result.duration / 3600 > 1 ? match.result.duration / 3600.0 : 0;
                TimeSummaries[3] += match.result.duration / 3600 > 1 ? Math.Truncate(match.result.duration / 3600.0) * 60 : (match.result.duration / 60) + ((match.result.duration / 60.0 - Math.Truncate(match.result.duration / 60.0)) * 60)/100.0;
            }
            for(int i = 0; i<3; i+=2)
            {
                while(TimeSummaries[i+1]>60)
                {
                    TimeSummaries[i + 1] -= 60;
                    TimeSummaries[i/1] += 1;
                }
            }
            Console.Write("First Player: hours - " + TimeSummaries[0] + " minutes - " +TimeSummaries[1] + " Second Player: hours - " + TimeSummaries[2] + " minutes - " + TimeSummaries[3]);
        }
    }
}
