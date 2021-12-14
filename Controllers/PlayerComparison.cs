using System;
using System.Collections.Generic;
using System.Text;
using DeserializeObjects;
using System.Linq;
using UserClasses;
using Newtonsoft.Json;

namespace Controllers
{
    public class PlayerComparison
    {

        public decimal FirstPlayerID, SecondPlayerID;
        public (GetMatchHistory.Root, GetMatchHistory.Root) Players;
        public (List<decimal>, List<decimal>) MatchIDs;
        public int time;
        public static List<GetMatchDetails.Root> GetMatchesDetailsListForTime(GetMatchHistory.Root Player, int time)
        {
            var matches = Player.result.Matches
                .Where(player => Deciphers.UnixTimeStampToDateTime(player.start_time) < DateTime.Now
                && Deciphers.UnixTimeStampToDateTime(player.start_time) > DateTime.Now.AddDays(-time)).ToList();
            var detailsList = new List<GetMatchDetails.Root>();
            foreach (var match in matches)
            {
                detailsList.Add(GetUrls.GetMatchDetailsUrl(match.MatchId));
            }
            return detailsList;
        }

        public PlayerComparison(decimal FirstPlayerID, decimal SecondPlayerID, int time)
        {
            Players = (GetUrls.GetMatchHistoryUrl(FirstPlayerID), GetUrls.GetMatchHistoryUrl(SecondPlayerID));
            this.time = time;
            this.FirstPlayerID = FirstPlayerID;
            this.SecondPlayerID = SecondPlayerID;
        }
        public static Dictionary<string, int> FillStats(int CountMatches, int Wins)
        {
            var Stats = new Dictionary<string, int>();
            Stats.Add("Player matches", CountMatches);
            Stats.Add("Player wins", Wins);
            Stats.Add("Player defeats", CountMatches - Wins);
            Stats.Add("Player winrate", (int)((double)Wins / CountMatches * 100));
            Stats.Add("Player MMR", Wins * 30 - (CountMatches - Wins) * 30);
            return Stats;
        }

        public Dictionary<string, int> WinRateAndRankComparison(decimal SteamID32)
        {
            var Wins = 0;
            var MatchesForTime = new List<GetMatchDetails.Root>();
            var deserializedData = GetUrls.GetMatchHistoryUrl(SteamID32);
            var matchDetailsList = new List<GetMatchDetails.Root>();
            var UserMatches = new List<Matches>();

            if (GetUserInfo.IsUserDataInDB(Deciphers.ConvertToSteamID64(SteamID32)))
            {
                using (var db = new UserContext())
                {
                    UserMatches = db.Matches.ToList().Where(user =>
                    user.User_SteamID == Deciphers.ConvertToSteamID64(SteamID32) &&
                    Deciphers.UnixTimeStampToDateTime(Convert.ToDouble(user.StartTime)) < DateTime.Now
                    && Deciphers.UnixTimeStampToDateTime(Convert.ToDouble(user.StartTime)) > DateTime.Now.AddDays(-time)).ToList();
                }
                foreach (var match in UserMatches)
                {
                    var details = JsonConvert.DeserializeObject<GetMatchDetails.Root>(match.DetailsData).result.players.Find(
                    player => player.account_id == SteamID32);

                    var PlayerSlot = Deciphers.PlayerSlotDecipher(details.player_slot);
                    Wins += Deciphers.WinChecker(PlayerSlot, JsonConvert.DeserializeObject<GetMatchDetails.Root>(match.DetailsData).result.radiant_win);
                }
                return FillStats(UserMatches.Count, Wins);
            }
            else
            {
                MatchesForTime = GetMatchesDetailsListForTime(GetUrls.GetMatchHistoryUrl(SteamID32), time);
                foreach (var match in MatchesForTime)
                {
                    var MatchWithPlayer = match.result.players.First(player =>
                    player.account_id == FirstPlayerID);
                    var PlayerSlot = Deciphers.PlayerSlotDecipher(MatchWithPlayer.player_slot);
                    Wins += Deciphers.WinChecker(PlayerSlot, match.result.radiant_win);
                }
                return FillStats(MatchesForTime.Count, Wins);
            }
        }

        public double[] TimeComparison(decimal SteamID32)
        {
            //0 - hours, 1 - minutes, 3 - SteamID64
            double[] TimeSummaries = { 0, 0, 0 };
            var MatchesForTime = new List<GetMatchDetails.Root>();
            var deserializedData = GetUrls.GetMatchHistoryUrl(SteamID32);
            var matchDetailsList = new List<GetMatchDetails.Root>();
            var UserMatches = new List<Matches>();

            TimeSummaries[2] = (int)SteamID32;
            if (GetUserInfo.IsUserDataInDB(Deciphers.ConvertToSteamID64(SteamID32)))
            {
                using (var db = new UserContext())
                {
                    UserMatches = db.Matches.ToList().Where(user =>
                    user.User_SteamID == Deciphers.ConvertToSteamID64(SteamID32) &&
                    Deciphers.UnixTimeStampToDateTime(Convert.ToDouble(user.StartTime)) < DateTime.Now
                    && Deciphers.UnixTimeStampToDateTime(Convert.ToDouble(user.StartTime)) > DateTime.Now.AddDays(-time)).ToList();
                }
                foreach (var match in UserMatches)
                {
                    /*Время приходит в секундах, при конвертации в минуты => 2254/60=37.56 получаем дробное значение, которое неправильно отображает время,
           так как 37.5666667 ~ 37 + (0.5666667*60)/100 = 37.34 (37 минут, 34 секунды), данные в массивы заносятся именно в таком формате*/
                    var MatchDetails = JsonConvert.DeserializeObject<GetMatchDetails.Root>(match.DetailsData);
                    TimeSummaries[0] += MatchDetails.result.duration / 3600.0 > 1 ? MatchDetails.result.duration / 3600.0 : 0;
                    TimeSummaries[1] += MatchDetails.result.duration / 3600 > 1 ? Math.Truncate(MatchDetails.result.duration / 3600.0) * 60 : (MatchDetails.result.duration / 60) + ((MatchDetails.result.duration / 60.0 - Math.Truncate(MatchDetails.result.duration / 60.0)) * 60) / 100.0;
                }
            }
            else
            {
                MatchesForTime = GetMatchesDetailsListForTime(GetUrls.GetMatchHistoryUrl(SteamID32), time);
                foreach (var match in MatchesForTime)
                {
                    TimeSummaries[0] += match.result.duration / 3600.0 > 1 ? match.result.duration / 3600.0 : 0;
                    TimeSummaries[1] += match.result.duration / 3600 > 1 ? Math.Truncate(match.result.duration / 3600.0) * 60 : (match.result.duration / 60) + ((match.result.duration / 60.0 - Math.Truncate(match.result.duration / 60.0)) * 60) / 100.0;
                }
            }
            while (TimeSummaries[1] > 60)
            {
                TimeSummaries[1] -= 60;
                TimeSummaries[0] += 1;
            }

            return TimeSummaries;
        }
    }
}
