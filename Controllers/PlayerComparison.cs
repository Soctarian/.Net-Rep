using System;
using System.Collections.Generic;
using System.Text;
using DeserializeObjects;
using System.Linq;
namespace Controllers
{
    public class PlayerComparison
    {

        public static List<decimal> GetMatchesArrayForTime(GetMatchHistory.Root Player, int time)
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

        /*Сравнение времени, проведенного в игре           План:
          Спарсить все матчи
          Отобрать айди тех, которые происходили за неделю\месяц\год\n-количество дней
          Спарсить все детали этих матчей и суммировать продолжительности игр
          Вывести время для 1 и 2 игра, провести необходимые сравнения
          Сравнения процента побед за определенное время и в общем    
        
                                                           План:
          Спарсить все матчи\Отобрать матчи за определенный промежуток времени
          Спарсить все детали этих матчей, определить в них нужных игроков, определить победил ли игрок
          Сумировать победы и поражения этих игроков, найти винрейты и их разность. 
          Вывести результат для обоих игроков 
         */
        public (GetMatchHistory.Root,GetMatchHistory.Root) Players;
        public (List<decimal>, List<decimal>) MatchIDs;
        public int time;
        public PlayerComparison(decimal FirstPlayerID, decimal SecondPlayerID, int time)
        {
            Players = (GetUrls.GetMatchHistoryUrl(FirstPlayerID), GetUrls.GetMatchHistoryUrl(SecondPlayerID));
            MatchIDs = (GetMatchesArrayForTime(Players.Item1, time), GetMatchesArrayForTime(Players.Item2, time));
            this.time = time;
        }
        
        public double[] TimeComparison()
        {   
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
                TimeSummaries[1] += match.result.duration / 3600 > 1 ? Math.Truncate(match.result.duration / 3600.0) * 60 : (match.result.duration / 60) + ((match.result.duration / 60.0 - Math.Truncate(match.result.duration / 60.0)) * 60) / 100.0;
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

            return TimeSummaries;
        }



    }
}
