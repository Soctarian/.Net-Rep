using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeserializeObjects
{
    public class GetPlayerSummaries
    {
        public class Player
        {
            [JsonProperty(PropertyName = "steamid")]
            public string SteamId { get; set; }

            [JsonProperty(PropertyName = "communityvisibilitystate")]
            public int VisibilityState { get; set; }

            [JsonProperty(PropertyName = "profilestate")]
            public int ProfileState { get; set; }

            [JsonProperty(PropertyName = "personaname")]
            public string Login { get; set; }

            [JsonProperty(PropertyName = "commentpermission")]
            public int CommentPermission { get; set; }

            [JsonProperty(PropertyName = "profileurl")]
            public string ProfileUrl { get; set; }

            public string Avatar { get; set; }
            public string avatarmedium { get; set; }
            public string avatarfull { get; set; }
            public string avatarhash { get; set; }

            [JsonProperty(PropertyName = "lastlogoff")]
            public int LastLogOff { get; set; }
            public int personastate { get; set; }
            public string realname { get; set; }

            [JsonProperty(PropertyName = "primaryclanid")]
            public string PrimaryClanId { get; set; }

            public int timecreated { get; set; }
            public int personastateflags { get; set; }
            public string loccountrycode { get; set; }
            public string locstatecode { get; set; }
        }
        public class Response
        {
            [JsonProperty(PropertyName = "players")]
            public List<Player> Players { get; set; }
        }
        public class Root
        {
            public Response response { get; set; }
        }


    }
}
