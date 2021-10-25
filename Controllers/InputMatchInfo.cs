using DeserializeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controllers
{
    public class InputMatchInfo
    {
        public InputMatchInfo(IOutputHandler outputHandler)
        {
            OutputHandler = outputHandler;
        }
        public InputMatchInfo()
        {

        }

        public static double WinRate;

        public IOutputHandler OutputHandler { get; }

        public void InputQuickMatchStatistic(int countMatches, decimal accountId)
        {
            var deserializedData = GetUrls.GetMatchHistoryUrl(accountId, countMatches);
            List<decimal> IDs = new List<decimal>();
            List<GetMatchDetails.Root> deserializedList = new List<GetMatchDetails.Root>();
            foreach (var match in deserializedData.result.Matches) IDs.Add(match.MatchId);
            var HeroDictionary = HeroAndItemsDictionary.FillHeroDictionary();
            string OutputResult = "";
            int WinCounts = 0;
            for (int i = 0; i < countMatches; i++)
            {
                OutputResult = $"{i + 1} match: Match ID - {IDs[i]}, Player team - ";
                deserializedList.Add(GetUrls.GetMatchDetailsUrl(IDs[i]));
                bool radiantWins = deserializedList[i].result.radiant_win;
                foreach (var player in deserializedList[i].result.players)
                {
                    if (player.account_id == accountId)
                    {
                        var PlayerSlot = Deciphers.PlayerSlotDecipher(player.player_slot);
                        switch (PlayerSlot["Team"])
                        {
                            case 0:
                                OutputResult += "Radiant";
                                WinCounts += radiantWins ? 1 : 0;
                                break;
                            case 1:
                                OutputResult += "Dire";
                                WinCounts += radiantWins ? 0 : 1;
                                break;
                        }
                        OutputResult += $", Hero - {HeroDictionary[player.hero_id]}, KDA = {Math.Round(((player.kills + player.assists) / (double)player.deaths), 1)}, GPM - {player.gold_per_min}, EPM - {player.xp_per_min}, Hero damage - {player.hero_damage}";
                    }
                }

                OutputHandler.Output(OutputResult);
            }
            WinRate = Math.Round(((WinCounts / (double)countMatches) * 100), 5);
            Console.WriteLine($"Winrate for {countMatches} matches: {WinRate}%");

        }

        public void InputFullkMatchStatistic(decimal MatchId)
        {
            var HeroDictionary = HeroAndItemsDictionary.FillHeroDictionary();
            string OutputResult = "Radiant:\n";
            var MatchObject = GetUrls.GetMatchDetailsUrl(MatchId);
            List<GetMatchDetails.Player> Players = MatchObject.result.players;
            int iter = 0;

            foreach (var player in Players)
            {
                var PlayerSteamID = player.account_id + 76561197960265728;
                if (player.account_id == UInt32.MaxValue) GetUserInfo.Login = "Anonymous";
                else GetUserInfo.DeterminatePlayerInfo(PlayerSteamID);

                if (iter == 5) OutputResult += "Dire:\n";
                OutputResult += $"{GetUserInfo.Login,15}  -\t{HeroDictionary[player.hero_id],10}\t{player.kills} / {player.deaths} / {player.assists}\t Net Worth : {player.net_worth}\n";
                iter++;
            }
            Console.Write(OutputResult);
        }

        public void InputDetailMatchStatistic(decimal MatchID, string Hero)
        {
            var ItemDictionary = HeroAndItemsDictionary.FillItemDictionary();
            var HeroDictionary = HeroAndItemsDictionary.FillHeroDictionary();
            string OutputResult = $"{Hero} stats:\n";
            var MatchObject = GetUrls.GetMatchDetailsUrl(MatchID);
            List<GetMatchDetails.Player> Players = MatchObject.result.players;
            GetMatchDetails.Player Player = Players.First(player => HeroDictionary[player.hero_id] == Hero);
            OutputResult += $"Player level : {Player.level}, GPM - {Player.gold_per_min}, EPM - {Player.xp_per_min}, Hero damage - {Player.hero_damage}, Tower Damage - {Player.tower_damage}, Hero healing - {Player.hero_healing}\n" +
                            $"Kills/Deaths/Assists : {Player.kills} / {Player.deaths} / {Player.assists}\t Net Worth : {Player.net_worth}\n" +
                            $"Krip stats : Last hits - {Player.last_hits}, Denies - {Player.denies}\n" +
                            $"Items:\n{ItemDictionary[Player.item_0],5}\t  |\t{ItemDictionary[Player.item_1],5}\t|\t{ItemDictionary[Player.item_2],5}\n{ItemDictionary[Player.item_3],5}\t  |\t{ItemDictionary[Player.item_4],5}\t|\t{ItemDictionary[Player.item_5],5}\nItem neutral: {ItemDictionary[Player.item_neutral]}\n";
            OutputResult += Player.aghanims_scepter == 0 ? "Aghanim scepter - No  " : "Aghanim scepter - Yes  ";
            OutputResult += Player.aghanims_shard == 0 ? "Aghanim shard - No  " : "Aghanim shard  - Yes";
            Console.Write(OutputResult);
        }


    }
}
