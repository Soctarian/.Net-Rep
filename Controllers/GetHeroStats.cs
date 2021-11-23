using System;
using System.Collections.Generic;
using System.Text;
using Controllers;
using System.Linq;
using DeserializeObjects;

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
                
                switch (PlayerSlot["Team"])
                {
                    case 0:

                        WinCounts += radiantWins ? 1 : 0;
                        OutputResult += radiantWins ? "Radiant, Win" : "Radiant, Loose";
                        break;
                    case 1:
                        WinCounts += radiantWins ? 0 : 1;
                        OutputResult += radiantWins ? "Dire, Loose" : "Dire, Win";
                        break;
                }
                OutputResult += $", KDA = {Math.Round(((ResultWithHero.kills + ResultWithHero.assists) / (double)ResultWithHero.deaths), 1)}, GPM - {ResultWithHero.gold_per_min}, EPM - {ResultWithHero.xp_per_min}, Hero damage - {ResultWithHero.hero_damage}";
                Console.WriteLine(OutputResult);
            }
            WinRate = Math.Round(WinCounts / (double)GameCount * 100, 5);
            Console.WriteLine($"Winrate for {GameCount} matches: {WinRate}%");

            //  return statsInfo;
        }

    }
}
