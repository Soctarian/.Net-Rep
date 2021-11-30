using System;
using System.Collections.Generic;
using System.Text;
using Controllers;
using System.Linq;
using DeserializeObjects;
using System.Threading.Tasks;

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

        //Нужно сохранить данные о матче и данные о игроке в этом матче

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

        public Dictionary<string, double> GetAverageHeroResults(decimal SteamID, string heroName)
        {
            var Result = new Dictionary<string, double>();
            var WinCounter = 0;
            var deserializedData = GetUrls.GetMatchHistoryUrl(Deciphers.ConvertToSteamID32(SteamID));
            var IDs = new List<decimal>();
            var detailsList = new List<GetMatchDetails.Player>();
            foreach (var match in deserializedData.result.Matches)
            {
                detailsList.Add(GetUrls.GetMatchDetailsUrl(match.MatchId).result.players.Find(
                    player => player.account_id == Deciphers.ConvertToSteamID32(SteamID) && 
                    this.HeroesAndItemsDictionaries.HeroDictionary[player.hero_id] == heroName));  
            }

            var averageMatchDetailsArray = new double[detailsList.Count];
           /* [0] - KDA
            * [1] - GPM
            * [2] - EPM
            * [3] - HeroDamage
            * [4] - NetWorth
            */
            foreach (var player in detailsList)
            {
                averageMatchDetailsArray[0] += (player.kills + player.assists) / (double)player.deaths;
                averageMatchDetailsArray[1] += player.gold_per_min;
                averageMatchDetailsArray[2] += player.xp_per_min;
                averageMatchDetailsArray[3] += player.hero_damage;
                averageMatchDetailsArray[4] += player.net_worth;
            }
            for (int i = 0; i < 5; i++) { averageMatchDetailsArray[i] /= (double)detailsList.Count; }

            Result.Add("KDA", averageMatchDetailsArray[0]);
            Result.Add("GPM", averageMatchDetailsArray[1]);
            Result.Add("EPM", averageMatchDetailsArray[2]);
            Result.Add("HeroDamage", averageMatchDetailsArray[3]);
            Result.Add("NetWorth", averageMatchDetailsArray[4]);

            return Result;
        }


    }
}
