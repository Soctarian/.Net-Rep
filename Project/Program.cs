using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using DeserializeObjects;
using UserClasses;
using System.Linq;

namespace Project
{
    class Program
    {
        public static double WinRate;
        static T GetWebResponseString<T>(Uri url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                    return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
        }
        static GetMatchHistory.Root GetMatchHistoryUrl(decimal accountId, int countMatches)
        {
            Uri url = new Uri($"https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/v1/?key=A80EC4AFFB0862E8476DFD2967292B79&account_id={accountId}&matches_requested={countMatches}");
            return GetWebResponseString<GetMatchHistory.Root>(url);
        }

        static GetMatchDetails.Root GetMatchDetailsUrl(decimal MatchId)
        {
            Uri url = new Uri($"https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/V001/?match_id={MatchId}&key=A80EC4AFFB0862E8476DFD2967292B79");
            return GetWebResponseString<GetMatchDetails.Root>(url);
        }

        static Dictionary<string, int> PlayerSlotDecipher(int data)
        {
            string threelastbites = " ";
            Dictionary<string, int> Team_Pos = new Dictionary<string, int>();
            string ConvertedData = Convert.ToString(data, 2).PadLeft(8, '0');
            char[] ConvertedDataArray = ConvertedData.ToCharArray();

            Team_Pos.Add("Team", ConvertedDataArray[0] - 48);

            for (int i = 7; i > 4; i--) threelastbites += ConvertedDataArray[i];
            int playerpos = Convert.ToInt32(Convert.ToSByte(threelastbites));
            Team_Pos.Add("Position", playerpos);

            return Team_Pos;
        }

        static void InputQuickMatchStatistic(List<decimal> IDs, List<GetMatchDetails.Root> deserializedList, int countMatches, decimal accountId)
        {
            var HeroDictionary = HeroAndItemsDictionary.FillHeroDictionary();
            string OutputResult = "";
            int WinCounts = 0;
            for (int i = 0; i < countMatches; i++)
            {
                OutputResult = $"{i + 1} match: Match ID - {IDs[i]}, Player team - ";
                deserializedList.Add(GetMatchDetailsUrl(IDs[i]));
                bool radiantWins = deserializedList[i].result.radiant_win;
                foreach (var player in deserializedList[i].result.players)
                {
                    if (player.account_id == accountId)
                    {
                        var PlayerSlot = PlayerSlotDecipher(player.player_slot);
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

                Console.WriteLine(OutputResult);
            }
            WinRate = Math.Round(((WinCounts / (double)countMatches) * 100), 5);
            Console.WriteLine($"Winrate for {countMatches} matches: {WinRate}%");
        }


        static void InputFullkMatchStatistic(decimal MatchId)
        {
            var HeroDictionary = HeroAndItemsDictionary.FillHeroDictionary();
            string OutputResult = "Radiant:\n";
            var MatchObject = GetMatchDetailsUrl(MatchId);
            List<GetMatchDetails.Player> Players = MatchObject.result.players;
            int iter = 0;

            foreach (var player in Players)
            {
                if (player.account_id == UInt32.MaxValue) GetUserInfo.Login = "Anonymous";
                else
                {
                    var PlayerSteamID = player.account_id + 76561197960265728;
                    GetUserInfo.DeterminatePlayerInfo(PlayerSteamID);
                }
                if (iter == 5) OutputResult += "Dire:\n";
                OutputResult += $"{iter+1}. {GetUserInfo.Login, 15}  -\t{HeroDictionary[player.hero_id],10}\t{player.kills} / {player.deaths} / {player.assists}\t Net Worth : {player.net_worth}\n";
                iter++; 
            }
            Console.Write(OutputResult);
        }

        static void InputDetailMatchStatistic(decimal MatchID, string Hero)
        {
            var ItemDictionary = HeroAndItemsDictionary.FillItemDictionary();
            var HeroDictionary = HeroAndItemsDictionary.FillHeroDictionary();
            string OutputResult = $"{Hero} stats:\n";
            var MatchObject = GetMatchDetailsUrl(MatchID);
            List<GetMatchDetails.Player> Players = MatchObject.result.players;
            GetMatchDetails.Player Player = Players.First(player => HeroDictionary[player.hero_id] == Hero);
            OutputResult += $"Player level : {Player.level}, GPM - {Player.gold_per_min}, EPM - {Player.xp_per_min}, Hero damage - {Player.hero_damage}, Tower Damage - {Player.tower_damage}, Hero healing - {Player.hero_healing}\n" +
                            $"Kills/Deaths/Assists : {Player.kills} / {Player.deaths} / {Player.assists}\t Net Worth : {Player.net_worth}\n" +
                            $"Krip stats : Last hits - {Player.last_hits}, Denies - {Player.denies}\n"+
                            $"Items:\n{ItemDictionary[Player.item_0],5}\t  |\t{ItemDictionary[Player.item_1],5}\t|\t{ItemDictionary[Player.item_2],5}\n{ItemDictionary[Player.item_3],5}\t  |\t{ItemDictionary[Player.item_4],5}\t|\t{ItemDictionary[Player.item_5],5}\nItem neutral: {ItemDictionary[Player.item_neutral]}\n";
            OutputResult += Player.aghanims_scepter == 0 ? "Aghanim scepter - No  " : "Aghanim scepter - Yes  ";
            OutputResult += Player.aghanims_shard == 0 ? "Aghanim scepter - No  " : "Aghanim shard  - Yes";
            Console.Write(OutputResult);
        }

        static void Main(string[] args)
        {
            //Test DotaID: 268677900  Test SteamID: 76561198228943628 Test GameID: 6229091942 ...

            Console.Write("Input profile ID: ");
            var accountId = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Input number of matches: ");
            var countMatches = Convert.ToInt32(Console.ReadLine());

            var deserializedData = GetMatchHistoryUrl(accountId, countMatches);
            List<decimal> IDs = new List<decimal>();
            List<GetMatchDetails.Root> deserializedList = new List<GetMatchDetails.Root>();

            //Клас, що буде ліпити урли + ключ та базову частину посилання та комбінувати об'єкти в правильні урли???
            foreach (var match in deserializedData.result.Matches) IDs.Add(match.MatchId);

            InputQuickMatchStatistic(IDs, deserializedList, countMatches, accountId);

            Console.Write("Input Match ID: ");
            var MatchID = Convert.ToDecimal(Console.ReadLine());
            InputFullkMatchStatistic(MatchID);

            var Hero = Console.ReadLine();
            InputDetailMatchStatistic(MatchID, Hero);


            Console.ReadKey();
        }
    }
}
