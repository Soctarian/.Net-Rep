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

        private HeroAndItemsDictionary HeroesAndItemsDictionaries;
        public GetHeroStats()
        {
            this.HeroesAndItemsDictionaries.FillHeroDictionary();
            this.HeroesAndItemsDictionaries.FillItemDictionary();
        }

        public Dictionary<string, int> GetHStats(long SteamID, string heroName)
        {
            var statsInfo = new Dictionary<string, int>();
            var matchesDetails = new GetUserInfo();
            IEnumerable<GetMatchDetails.Player> Players;

            var MatchesWithHero = matchesDetails.DetailsList;

            foreach(var match in MatchesWithHero)
            {
                Players = match.result.players.Where(player =>
                player.account_id == Deciphers.ConvertToSteamID32(SteamID) &&
                this.HeroesAndItemsDictionaries.HeroDictionary[player.hero_id] == heroName);
            }
            

            return statsInfo;
        }

    }
}
