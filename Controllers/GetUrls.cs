using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeserializeObjects
{
    public class GetUrls
    {
        static readonly HttpClient client = new HttpClient();
        public static T GetUserString<T>(decimal UserSteamID)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=A80EC4AFFB0862E8476DFD2967292B79&steamids={UserSteamID}");
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                    return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
        }
        public static T GetWebResponseString<T>(Uri url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                    return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
        }
        public static async Task<T> GetWebResponseStringAsync<T>(Uri url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                    return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
        }
        public static string GetResponseString(Uri url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                    return sr.ReadToEnd();
            }
        }


        //Test threading. Failed cause its not possible to send too many requests
        /*  public static async Task<T> GetWebResponseStringAsync<T>(Uri url)
          {
              using (HttpResponseMessage response = await client.GetAsync(url))
              {
                  return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync()); 
              }
          }*/

        public static GetMatchHistory.Root GetMatchHistoryUrl(decimal accountID, int countMatches)
        {
            Uri url = new Uri($"https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/v1/?key=A80EC4AFFB0862E8476DFD2967292B79&account_id={accountID}&matches_requested={countMatches}");
            return GetWebResponseString<GetMatchHistory.Root>(url);
        }

        public static GetMatchHistory.Root GetMatchHistoryUrl(decimal accountID)
        {
            Uri url = new Uri($"https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/v1/?key=A80EC4AFFB0862E8476DFD2967292B79&account_id={accountID}");
            return GetWebResponseString<GetMatchHistory.Root>(url);
        }

        public static GetMatchDetails.Root GetMatchDetailsUrl(decimal MatchId)
        {
            Uri url = new Uri($"https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/V001/?match_id={MatchId}&key=A80EC4AFFB0862E8476DFD2967292B79");
            return GetWebResponseString<GetMatchDetails.Root>(url);
        }
        public static async Task<GetMatchDetails.Root> GetMatchDetailsUrlAsync(decimal MatchId)
        {
            Uri url = new Uri($"https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/V001/?match_id={MatchId}&key=A80EC4AFFB0862E8476DFD2967292B79");
            return await GetWebResponseStringAsync<GetMatchDetails.Root>(url);
        }
        public static string GetMatchDetailsString(decimal MatchID)
        {
            Uri url = new Uri($"https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/V001/?match_id={MatchID}&key=A80EC4AFFB0862E8476DFD2967292B79");
            return GetResponseString(url);
        }
    }
}
