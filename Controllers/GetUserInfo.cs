using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using DeserializeObjects;
using Newtonsoft.Json;
using UserClasses;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Controllers
{
    public class GetUserInfo
    {

        public List<GetMatchDetails.Root> DetailsList = new List<GetMatchDetails.Root>();
        public decimal UserSteamID64 { get; set; }
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

        public GetUserInfo(decimal SteamID)
        {
            this.UserSteamID64 = SteamID;
        }
        public GetUserInfo() { }

        public void DeterminatePlayerInfo()
        {
            var DeserializedObject = GetUrls.GetUserString<GetPlayerSummaries.Root>(this.UserSteamID64);
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
        public void GetDetailsFromDB(decimal SteamID)
        {
            var detailsList = new List<GetMatchDetails.Root>();
            var userMatches = new List<Matches>();
            using (var db = new UserContext())
            {
                userMatches = db.Matches.AsNoTracking().Where(userid => userid.User_SteamID == SteamID).ToList();
            }
            foreach(var match in userMatches)
            {
                DetailsList.Add(JsonConvert.DeserializeObject<GetMatchDetails.Root>(match.DetailsData));
            }
        } 

        public static bool IsUserDataInDB(decimal SteamID)
        {
            var user = new User();
           using(var db = new UserContext())
           {
                user = db.Users.AsNoTracking().First(user => user.SteamID == SteamID);
           }
            return user != null ? true : false;
        }
    
    }
}
