using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using DeserializeObjects;
using Newtonsoft.Json;

namespace UserClasses
{
    public class GetUserInfo
    {
        public static string Login { get; set; }
        public static string Status { get; set; }
        public static DateTime LastLogOff { get; set; }

        static T GetUserString<T>(decimal UserSteamID)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=A80EC4AFFB0862E8476DFD2967292B79&steamids={UserSteamID}");
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                    return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
        }
        static public void DeterminatePlayerInfo(decimal UserSteamID)
        {
            var DeserializedObject = GetUserString<GetPlayerSummaries.Root>(UserSteamID);
            var Player = DeserializedObject.response.Players;
            foreach(var user in Player)
            {
                Login = user.Login;
                Status = PlayerStatusDecipher(user.personastate);
                LastLogOff = user.personastate == 0 ? UnixTimeStampToDateTime(user.LastLogOff) : DateTime.Now;
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        static string PlayerStatusDecipher(int ProfileState)
        {
            Dictionary<int, string> PlayerStatus = new Dictionary<int, string>();
            PlayerStatus.Add(0, "Offline");
            PlayerStatus.Add(1, "Online");
            PlayerStatus.Add(2, "Busy");
            PlayerStatus.Add(3, "Away");
            PlayerStatus.Add(4, "Snooze");
            PlayerStatus.Add(5, "Looking for trade");
            PlayerStatus.Add(6, "Looking for play");
            return PlayerStatus[ProfileState];
        }


    }
}
