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
            string threelastbites=" ";
            Dictionary<string, int> Team_Pos = new Dictionary<string, int>();
            string ConvertedData = Convert.ToString(data, 2).PadLeft(8, '0');
            char[] ConvertedDataArray = ConvertedData.ToCharArray();

            Team_Pos.Add("Team", ConvertedDataArray[0]-48);

            for (int i = 7; i > 4; i--) threelastbites += ConvertedDataArray[i];
            int playerpos = Convert.ToInt32(Convert.ToSByte(threelastbites));
            Team_Pos.Add("Position", playerpos);
            
            return Team_Pos; 
        }

        static void InputQuickMatchStatistic(List<decimal> IDs, Dictionary<int, string> HeroDictionary, List<GetMatchDetails.Root> deserializedList, int countMatches, decimal accountId)
        {
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
                        OutputResult += $", Hero - {HeroDictionary[player.hero_id]}, KDA = {Math.Round(((player.kills+player.assists)/(double)player.deaths),1)}, GPM - {player.gold_per_min}, EPM - {player.xp_per_min}, Hero damage - {player.hero_damage}";
                    }
                }

                Console.WriteLine(OutputResult);
            }
            WinRate = Math.Round(((WinCounts / (double)countMatches) * 100), 5);
            Console.WriteLine($"Winrate for {countMatches} matches: {WinRate}%");
        }

        static void InputFullkMatchStatistic(Dictionary<int, string> HeroDictionary, List<GetMatchDetails.Root> deserializedList, decimal MatchId)
        {

        }

        static void Main(string[] args)
        {
            Console.Write("Input profile ID: ");
            var accountId = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Input number of matches: ");
            var countMatches = Convert.ToInt32(Console.ReadLine());

            var deserializedData = GetMatchHistoryUrl(accountId, countMatches);
            List<decimal> IDs = new List<decimal>();
            List<GetMatchDetails.Root> deserializedList = new List<GetMatchDetails.Root>();

            //Initialization of dictionaries 
            HeroAndItemsDictionary fillDictonary = new HeroAndItemsDictionary();
            var HeroDictionary = fillDictonary.FillHeroDictionary(); //Dictonary with <id, name> 
            var ItemDictionary = fillDictonary.FillItemDictionary();

            //Клас, що буде ліпити урли + ключ та базову частину посилання та комбінувати об'єкти в правильні урли???

            foreach (var match in deserializedData.result.Matches) IDs.Add(match.MatchId);

            InputQuickMatchStatistic(IDs, HeroDictionary, deserializedList, countMatches, accountId);

            Console.Write("Input Steam ID: ");
            var SteamID = Convert.ToDecimal(Console.ReadLine());
            GetUserInfo.DeterminatePlayerInfo(SteamID);

            Console.Write($"\n Info: Login - {GetUserInfo.Login}, Status - {GetUserInfo.Status}, Last log off - {GetUserInfo.LastLogOff}");


            


            Console.ReadKey();
        }
    }
}
