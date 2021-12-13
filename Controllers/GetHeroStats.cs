using System;
using System.Collections.Generic;
using System.Text;
using Controllers;
using System.Linq;
using DeserializeObjects;
using System.Threading.Tasks;
using UserClasses;
using Newtonsoft.Json;

namespace Controllers
{
    class GetHeroStats
    {

        private HeroAndItemsDictionary HeroesAndItemsDictionaries = new HeroAndItemsDictionary();
        public GetHeroStats()
        {
            this.HeroesAndItemsDictionaries.FillHeroDictionary();
            this.HeroesAndItemsDictionaries.FillItemDictionary();
        }

        private int CheckPlayerWin(GetMatchDetails.Player player, GetMatchDetails.Root match)
        {
            int win = 0;

            var PlayerSlot = Deciphers.PlayerSlotDecipher(player.player_slot);
            var radiantWins = match.result.radiant_win;
            switch (PlayerSlot["Team"])
            {
                case 0:

                    win = radiantWins ? 1 : 0;
                    break;
                case 1:
                    win = radiantWins ? 0 : 1;
                    break;
            }

            return win;
        }

        public void GetHStats(long SteamID, string heroName, GetUserInfo getuserinfo)
        {
            var statsInfo = new Dictionary<string, int>();
            GetMatchDetails.Player ResultWithHero;
            string OutputResult = "";

            int WinCounts = 0, GameCount=0;
            double WinRate = 0;
            var AllMatches = getuserinfo.DetailsList;

            foreach (var match in AllMatches)
            {

                bool radiantWins = match.result.radiant_win;
                ResultWithHero = match.result.players.Find(player =>
                player.account_id == Deciphers.ConvertToSteamID32(SteamID) &&
                this.HeroesAndItemsDictionaries.HeroDictionary[player.hero_id] == heroName);
                if (ResultWithHero == null) continue;
                OutputResult= $"Match ID - {match.result.match_id}, Player team - ";
                GameCount++;
                var PlayerSlot = Deciphers.PlayerSlotDecipher(ResultWithHero.player_slot);

                var outputmatchinfo = new OutputMatchInfo();
                var result = outputmatchinfo.QuickMatchStatisticResultString(PlayerSlot, radiantWins, ResultWithHero);

                Console.WriteLine(OutputResult + result["InfoResult"]);
                WinCounts += Convert.ToInt32(result["WinCount"]);

            }
            WinRate = Math.Round(WinCounts / (double)GameCount * 100, 5);
            Console.WriteLine($"Winrate for {GameCount} matches: {WinRate}%");
        }

        public async Task<Dictionary<string, double>> GetAverageHeroResultsAsync(decimal SteamID32, string heroName)
        {
            var Result = new Dictionary<string, double>();
            var searchedPlayerDetailsList = new List<GetMatchDetails.Player>();
            var deserializedData = GetUrls.GetMatchHistoryUrl(SteamID32);
            var matchDetailsList = new List<GetMatchDetails.Root>();
            int WinCounter = 0;
            bool radiantWins;
            var UserMatches = new List<Matches>();

          
            if (GetUserInfo.IsUserDataInDB(Deciphers.ConvertToSteamID64(SteamID32)))
            {
                using (var db = new UserContext())
                {
                    UserMatches = db.Matches.ToList().Where(user =>
                    user.User_SteamID == Deciphers.ConvertToSteamID64(SteamID32)).ToList();
                }
                foreach (var match in UserMatches)
                {
                    var details = JsonConvert.DeserializeObject<GetMatchDetails.Root>(match.DetailsData).result.players.Find(
                     player => player.account_id == SteamID32 &&
                     this.HeroesAndItemsDictionaries.HeroDictionary[player.hero_id] == heroName);

                    if (details != null)
                    {
                        searchedPlayerDetailsList.Add(details);
                        WinCounter += CheckPlayerWin(details, JsonConvert.DeserializeObject<GetMatchDetails.Root>(match.DetailsData));
                    }
                }
            }
            else
            {
                foreach (var match in deserializedData.result.Matches)
                {
                    matchDetailsList.Add(GetUrls.GetMatchDetailsUrl(match.MatchId));
                }
                foreach (var match in matchDetailsList)
                {
                    var details = match.result.players.Find(
                         player => player.account_id == SteamID32 &&
                         this.HeroesAndItemsDictionaries.HeroDictionary[player.hero_id] == heroName);

                    if (details != null)
                    {
                        searchedPlayerDetailsList.Add(details);
                        WinCounter += CheckPlayerWin(details, match);
                    }
                }
            }

            var averageMatchDetailsArray = new double[6];
            for (int i = 0; i < 6; i++) averageMatchDetailsArray[i] = 0;
            /* [0] - KDA
             * [1] - GPM
             * [2] - EPM
             * [3] - HeroDamage
             * [4] - NetWorth
             * [5] - WinRate
             */
            averageMatchDetailsArray[5] = WinCounter;
            foreach (var player in searchedPlayerDetailsList)
            {
                averageMatchDetailsArray[0] += player.deaths != 0 ? (player.kills + player.assists) / (double)player.deaths : (player.kills + player.assists);
                averageMatchDetailsArray[1] += player.gold_per_min;
                averageMatchDetailsArray[2] += player.xp_per_min;
                averageMatchDetailsArray[3] += player.hero_damage;
                averageMatchDetailsArray[4] += player.net_worth;
            }
            for (int i = 0; i < 6; i++) { averageMatchDetailsArray[i] /= (double)searchedPlayerDetailsList.Count; }

            Result.Add("KDA", Math.Round(averageMatchDetailsArray[0], 2));
            Result.Add("GPM", Math.Round(averageMatchDetailsArray[1]));
            Result.Add("EPM", Math.Round(averageMatchDetailsArray[2]));
            Result.Add("HeroDamage", Math.Round(averageMatchDetailsArray[3]));
            Result.Add("NetWorth", Math.Round(averageMatchDetailsArray[4]));
            Result.Add("WinRate", Math.Round(averageMatchDetailsArray[5] * 100));

            return Result;
        }
    }
}
