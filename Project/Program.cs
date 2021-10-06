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
using ClassLibrariesTest;

namespace Project
{
    class Program
    {

        static string GetMatchHistoryString(long accountId, int countMatches)
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

        static void Main(string[] args)
        {
            long accountId;
            int countMatches;
            Console.Write("Введите Id профиля: ");
            accountId = Convert.ToInt64(Console.ReadLine());
            Console.Write("Введите количество матчей: ");
            countMatches = Convert.ToInt32(Console.ReadLine());

            

            var deserializedData = JsonConvert.DeserializeObject<Class1.Root>(GetMatchHistoryString(accountId, countMatches));
            
            Console.Write("Id матчей: ");
            foreach (var match in deserializedData.result.matches)
            {
                Console.WriteLine(match.MatchId);
            }

            Console.ReadKey();
        }
    }
}
