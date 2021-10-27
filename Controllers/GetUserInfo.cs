using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using DeserializeObjects;
using Newtonsoft.Json;
using Controllers;
namespace DeserializeObjects
{
    public class GetUserInfo
    {
        public decimal UserSteamID32 { get; set; }
        public string Login { get; set; }
        public string Status { get; set; }
        public DateTime LastLogOffDT { get; set; }
        public int CommunityVisible { get; set; }
        public int CommentPermission { get; set; }
        public string ProfileUrl { get; set; }
        public string Avatar { get; set; }
        public decimal LastLogOff { get; set; }
        public decimal TimeCreated { get; set; }
        public string RealName { get; set; }

        public T GetUserString<T>(decimal UserSteamID)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=A80EC4AFFB0862E8476DFD2967292B79&steamids={UserSteamID}");
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                    return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
        }
        public void DeterminatePlayerInfo(decimal UserSteamID)
        {
            var DeserializedObject = GetUserString<GetPlayerSummaries.Root>(UserSteamID);
            var Player = DeserializedObject.response.Players;
            foreach (var user in Player)
            {
                Login = user.Login;
                Status = Deciphers.PlayerStatusDecipher(user.personastate);
                LastLogOffDT = user.personastate == 0 ? Deciphers.UnixTimeStampToDateTime(user.LastLogOff) : DateTime.Now;
                CommunityVisible = user.CommunityVisible;
                CommentPermission = user.CommentPermission;
                ProfileUrl = user.ProfileUrl;
                Avatar = user.Avatar;
                LastLogOff = user.LastLogOff;
                TimeCreated = user.TimeCreated;
                RealName = user.RealName;
                UserSteamID32 = user.SteamID - 76561197960265728; 
            }
        }


    }
}
