using System;
using System.Collections.Generic;
using System.Text;
using DeserializeObjects;
using System.Linq;
namespace Controllers
{
    public class PlayerComparison
    {

        private decimal FirstPlayerID, SecondPlayerID;
        public (GetMatchHistory.Root, GetMatchHistory.Root) Players;
        public (List<decimal>, List<decimal>) MatchIDs;
        public int time;
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
        public PlayerComparison(decimal FirstPlayerID, decimal SecondPlayerID, int time)
        {
            Players = (GetUrls.GetMatchHistoryUrl(FirstPlayerID), GetUrls.GetMatchHistoryUrl(SecondPlayerID));
            MatchIDs = (GetMatchesArrayForTime(Players.Item1, time), GetMatchesArrayForTime(Players.Item2, time));
            this.time = time;
            this.FirstPlayerID = FirstPlayerID;
            this.SecondPlayerID = SecondPlayerID;
        }
        public static Dictionary<string, int> FillStats(int CountMatches, int Wins)
        {
            var Stats = new Dictionary<string, int>();
            Stats.Add("Player matches", CountMatches);
            Stats.Add("Player wins", Wins);
            Stats.Add("Player defeats", CountMatches-Wins);
            Stats.Add("Payer winrate", (int)((double)Wins/CountMatches*100));
            Stats.Add("Player MMR", Wins * 30 - (CountMatches - Wins) * 30);
            return Stats;
        }

        public Dictionary<string, int> WinRateAndRankComparison()
        {
            
            (int,int) Wins = (0, 0);
            List<GetMatchDetails.Root> FirstListMatches = new List<GetMatchDetails.Root>();
            List<GetMatchDetails.Root> SecondListMatches = new List<GetMatchDetails.Root>();
            foreach (decimal ID in MatchIDs.Item1) FirstListMatches.Add(GetUrls.GetMatchDetailsUrl(ID));
            foreach (decimal ID in MatchIDs.Item2) SecondListMatches.Add(GetUrls.GetMatchDetailsUrl(ID));
            (int, int) CountMatches = (FirstListMatches.Count, SecondListMatches.Count);
            foreach (var match in FirstListMatches)
            {
                GetMatchDetails.Player Player = match.result.players.First(player => player.account_id == FirstPlayerID);
                var PlayerSlot = Deciphers.PlayerSlotDecipher(Player.player_slot);
                Wins.Item1 += Deciphers.WinChecker(PlayerSlot, match.result.radiant_win);
            }
            foreach (var match in SecondListMatches)
            {
                GetMatchDetails.Player Player = match.result.players.First(player => player.account_id == SecondPlayerID);
                var PlayerSlot = Deciphers.PlayerSlotDecipher(Player.player_slot);
                Wins.Item2 += Deciphers.WinChecker(PlayerSlot, match.result.radiant_win);
            }

            return FillStats(0, 0);
        }

        public double[] TimeComparison()
        {   
            List<GetMatchDetails.Root> FirstListMatches = new List<GetMatchDetails.Root>();
            List<GetMatchDetails.Root> SecondListMatches = new List<GetMatchDetails.Root>();
            //0 - hours, 1 - minutes for First Player, 2 - hours, 3 - minutes for Second Player
            double[] TimeSummaries = { 0, 0, 0, 0, 0, 0 };
            foreach (decimal ID in MatchIDs.Item1) FirstListMatches.Add(GetUrls.GetMatchDetailsUrl(ID));
            foreach (decimal ID in MatchIDs.Item2) SecondListMatches.Add(GetUrls.GetMatchDetailsUrl(ID));
            TimeSummaries[4] = Convert.ToInt32(FirstPlayerID);
            TimeSummaries[5] = Convert.ToInt32(SecondPlayerID);
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
