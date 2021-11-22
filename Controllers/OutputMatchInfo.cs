using DeserializeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controllers
{
    public class OutputMatchInfo
    {
       
        private HeroAndItemsDictionary HeroesAndItemsDictionaries = new HeroAndItemsDictionary();
        public OutputMatchInfo()
        {
            this.HeroesAndItemsDictionaries.FillHeroDictionary();
            this.HeroesAndItemsDictionaries.FillItemDictionary();
        }
  
        private static double WinRate;

        public void InputQuickMatchStatistic(int countMatches, decimal accountId)
        {
            var deserializedData = GetUrls.GetMatchHistoryUrl(accountId, countMatches);
            var IDs = new List<decimal>();
            List<GetMatchDetails.Root> deserializedList = new List<GetMatchDetails.Root>();
            foreach (var match in deserializedData.result.Matches) IDs.Add(match.MatchId);

            string OutputResult = "";
            int WinCounts = 0;
            for (int i = 0; i < countMatches; i++)
            {
                OutputResult = $"{i + 1} match: Match ID - {IDs[i]}, Player team - ";
                deserializedList.Add(GetUrls.GetMatchDetailsUrl(IDs[i]));
                bool radiantWins = deserializedList[i].result.radiant_win;
                GetMatchDetails.Player player = deserializedList[i].result.players.First(player => player.account_id == accountId);
                var PlayerSlot = Deciphers.PlayerSlotDecipher(player.player_slot);
                switch (PlayerSlot["Team"])
                {
                    case 0:

                        WinCounts += radiantWins ? 1 : 0;
                        OutputResult += radiantWins ? "Radiant, Win" : "Radiant, Loose";
                        break;
                    case 1:
                        OutputResult += "Dire";
                        WinCounts += radiantWins ? 0 : 1;
                        OutputResult += radiantWins ? "Dire, Loose" : "Dire, Win";
                        break;
                }
                OutputResult += $", Hero - {this.HeroesAndItemsDictionaries.HeroDictionary[player.hero_id]}, KDA = {Math.Round(((player.kills + player.assists) / (double)player.deaths), 1)}, GPM - {player.gold_per_min}, EPM - {player.xp_per_min}, Hero damage - {player.hero_damage}";


                Console.WriteLine(OutputResult);
            }
            WinRate = Math.Round(((WinCounts / (double)countMatches) * 100), 5);
            Console.WriteLine($"Winrate for {countMatches} matches: {WinRate}%");

        }

        public void InputFullkMatchStatistic(decimal MatchID)
        {
            GetUserInfo user = new GetUserInfo();

            string OutputResult = "Radiant:\n";
            var MatchObject = GetUrls.GetMatchDetailsUrl(MatchID);
            List<GetMatchDetails.Player> Players = MatchObject.result.players;
            int iter = 0;

            foreach (var player in Players)
            {
                var PlayerSteamID = player.account_id + 76561197960265728;
                if (player.account_id == UInt32.MaxValue) user.Login = "Anonymous";
                else user.DeterminatePlayerInfo(PlayerSteamID);

                if (iter == 5) OutputResult += "Dire:\n";
                OutputResult += $"{user.Login,15}  -\t{this.HeroesAndItemsDictionaries.HeroDictionary[player.hero_id],10}\t{player.kills} / {player.deaths} / {player.assists}\t Net Worth : {player.net_worth}\n";
                iter++;
            }
            Console.Write(OutputResult);
        }

        public void InputDetailMatchStatistic(string Hero, decimal matchID)
        {
            string OutputResult = $"{Hero} stats:\n";
            var MatchObject = GetUrls.GetMatchDetailsUrl(matchID);
            List<GetMatchDetails.Player> Players = MatchObject.result.players;
            GetMatchDetails.Player Player = Players.First(player => this.HeroesAndItemsDictionaries.HeroDictionary[player.hero_id] == Hero);
            OutputResult += $"Player level : {Player.level}, GPM - {Player.gold_per_min}, EPM - {Player.xp_per_min}, Hero damage - {Player.hero_damage}, Tower Damage - {Player.tower_damage}, Hero healing - {Player.hero_healing}\n" +
                            $"Kills/Deaths/Assists : {Player.kills} / {Player.deaths} / {Player.assists}\t Net Worth : {Player.net_worth}\n" +
                            $"Krip stats : Last hits - {Player.last_hits}, Denies - {Player.denies}\n" +
                            $"Items:\n{this.HeroesAndItemsDictionaries.ItemDictionary[Player.item_0],5}\t  |\t{this.HeroesAndItemsDictionaries.ItemDictionary[Player.item_1],5}\t" +
                            $"|\t{this.HeroesAndItemsDictionaries.ItemDictionary[Player.item_2],5}\n{this.HeroesAndItemsDictionaries.ItemDictionary[Player.item_3],5}\t  |\t" +
                            $"{this.HeroesAndItemsDictionaries.ItemDictionary[Player.item_4],5}\t|\t{this.HeroesAndItemsDictionaries.ItemDictionary[Player.item_5],5}\nItem neutral: " +
                            $"{this.HeroesAndItemsDictionaries.ItemDictionary[Player.item_neutral]}\n";
            OutputResult += Player.aghanims_scepter == 0 ? "Aghanim scepter - No  " : "Aghanim scepter - Yes  ";
            OutputResult += Player.aghanims_shard == 0 ? "Aghanim shard - No  " : "Aghanim shard  - Yes";
            Console.Write(OutputResult);
        }


    }
}
