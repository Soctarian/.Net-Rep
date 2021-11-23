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

namespace Controllers
{
    

    public class GetUserInfo
    {

        public List<GetMatchDetails.Root> DetailsList = new List<GetMatchDetails.Root>();
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


        public void DeterminatePlayerInfo(decimal UserSteamID)
        {
            var DeserializedObject = GetUrls.GetUserString<GetPlayerSummaries.Root>(UserSteamID);
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

        public static List<decimal> GetPlayersMatches(decimal SteamID)
        {
            var matchesList = new List<decimal>();
            using (var db = new UserContext())
            {
                var matches = db.Matches.Where(match => match.User_SteamID == SteamID);
                foreach (var match in matches) matchesList.Add(match.MatchID);

            }
            return matchesList;
        }

        public void GetDetailsList(decimal SteamID)
        {
            var detailsList = new List<GetMatchDetails.Root>();
            var deserializedData = GetUrls.GetMatchHistoryUrl(Deciphers.ConvertToSteamID32(SteamID));
            foreach(var match in deserializedData.result.Matches)
            {
                DetailsList.Add(GetUrls.GetMatchDetailsUrl(match.MatchId));
            }

          //  return detailsList;
        }
  /*      public async void GetDetailsListAsync(decimal SteamID)
        {
             DetailsList = await Task.Run(() => GetDetailsList(SteamID));
        }*/

    }
}
