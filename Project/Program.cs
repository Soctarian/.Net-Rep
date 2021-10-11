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

        static string GetMatchHistoryString(decimal accountId, int countMatches)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
             $"https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/v1/?key=A80EC4AFFB0862E8476DFD2967292B79&account_id={accountId}&matches_requested={countMatches}");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string sReadData;

            return sReadData = sr.ReadToEnd();
            //response.Close();
        }
        static string GetMatchDetailsString(decimal MatchId)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
             $"https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/V001/?match_id={MatchId}&key=A80EC4AFFB0862E8476DFD2967292B79");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string sReadData;

            return sReadData = sr.ReadToEnd();
            //response.Close();
        }

        static void Main(string[] args)
        {
            decimal accountId;
            int countMatches;
            Console.Write("Input profile ID: ");
            accountId = Convert.ToDecimal(Console.ReadLine());//Convert.ToInt64(Console.ReadLine());
            Console.Write("Input number of matches: ");
            countMatches = Convert.ToInt32(Console.ReadLine());

            var deserializedData = JsonConvert.DeserializeObject<GetMatchHistory.Root>(GetMatchHistoryString(accountId, countMatches));
            List<decimal> IDs = new List<decimal>();

            Console.WriteLine();
            Console.Write("Matches IDs: ");
            foreach (var match in deserializedData.result.Matches)
            {
                IDs.Add(match.MatchId);
                Console.WriteLine(match.MatchId);
            }

            List<GetMatchDetails.Root> deserializedList = new List<GetMatchDetails.Root>();
            string PlayerTeam;
            string ms = "";
            bool win;
            for (int i = 0; i < countMatches; i++)
            {
                ms = $"{i+1} match: Player team - ";
                deserializedList.Add(JsonConvert.DeserializeObject<GetMatchDetails.Root>(GetMatchDetailsString(IDs[i])));
                bool radiantWins = deserializedList[i].result.radiant_win;
                foreach (var player in deserializedList[i].result.players)
                {
                    if (player.account_id == accountId)
                    {
                        //Нужно еще сконвертить player.player_slot в двоичную систему, добить до 8битного числа и считать 1 цифру. 0 - Radiant, 1 - Dire  :/
                        switch (player.player_slot)
                        {
                            case 0:
                                ms += "Radiant";
                                win = radiantWins ? true : false;
                                break;
                            case 1:
                                ms += "Dire";
                                win = radiantWins ? false : true;
                                break;

                        }
                    }
                }
                Console.WriteLine(ms);
            }
           

            //Инициализация словаря id - hero_name
            HeroDictionary fillDictonary = new HeroDictionary();
            string HeroString = fillDictonary.GetHeroString();
            fillDictonary.FillDictionary(HeroString);



            Console.ReadKey();
        }
    }
}
