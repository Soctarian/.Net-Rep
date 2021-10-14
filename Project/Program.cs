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


namespace Project
{
    class Program
    {

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

        //Винести розшифровку plater_slot в окремий метод який буде повертати структуру з усією информацією 

        static void Main(string[] args)
        {
            Console.Write("Input profile ID: ");
            var accountId = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Input number of matches: ");
            var countMatches = Convert.ToInt32(Console.ReadLine());

            var deserializedData = GetMatchHistoryUrl(accountId, countMatches);
            List<decimal> IDs = new List<decimal>();

            //Клас, що буде ліпити урли + ключ та базову частину посилання та комбінувати об'єкти в правильні урли

            Console.WriteLine();
            Console.Write("Matches IDs: ");
            foreach (var match in deserializedData.result.Matches)
            {
                IDs.Add(match.MatchId);
                Console.Write(match.MatchId + " ");
            }



            List<GetMatchDetails.Root> deserializedList = new List<GetMatchDetails.Root>();
            string PlayerTeam;
            string ms = "";
            int WinCounts=0;
            for (int i = 0; i < countMatches; i++)
            {
                ms = $"{i + 1} match: Player team - ";
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
                                ms += "Radiant";
                                WinCounts += radiantWins ? 1 : 0;
                                break;
                            case 1:
                                ms += "Dire";
                                WinCounts += radiantWins ? 0 : 1;
                                break;

                        }
                    }
                }

                Console.WriteLine(ms);
            }
            double WinRate = Math.Round((WinCounts / (double)countMatches)*100);
            Console.WriteLine($"Winrate for {countMatches}: {WinRate}%");

            //Инициализация словаря id - hero_name
            HeroDictionary fillDictonary = new HeroDictionary();
            string HeroString = fillDictonary.GetHeroString();
            fillDictonary.FillDictionary(HeroString);



            Console.ReadKey();
        }
    }
}
