using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DeserializeObjects
{
    public class GetUrls
    {
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
        public static GetMatchHistory.Root GetMatchHistoryUrl(decimal accountId, int countMatches)
        {
            Uri url = new Uri($"https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/v1/?key=A80EC4AFFB0862E8476DFD2967292B79&account_id={accountId}&matches_requested={countMatches}");
            return GetWebResponseString<GetMatchHistory.Root>(url);
        }

        public static GetMatchDetails.Root GetMatchDetailsUrl(decimal MatchId)
        {
            Uri url = new Uri($"https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/V001/?match_id={MatchId}&key=A80EC4AFFB0862E8476DFD2967292B79");
            return GetWebResponseString<GetMatchDetails.Root>(url);
        }


    }
}
