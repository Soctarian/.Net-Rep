using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DeserializeObjects
{
    public class GetMatchHistory
    {

        public class Player
        {
            [JsonProperty(PropertyName = "account_id")]
            public object AccountId { get; set; }

            [JsonProperty(PropertyName = "player_slot")]
            public int PlayerSlot { get; set; }

            [JsonProperty(PropertyName = "hero_id")]
            public int HeroId { get; set; }
        }


        public class Match
        {
            [JsonProperty(PropertyName = "match_id")]
            public decimal MatchId { get; set; }
            public object match_seq_num { get; set; }
            public int start_time { get; set; }
            public int lobby_type { get; set; }
            public int radiant_team_id { get; set; }
            public int dire_team_id { get; set; }
            public List<Player> players { get; set; }
        }

        public class Result
        {
            public int status { get; set; }
            public int num_results { get; set; }
            public int total_results { get; set; }
            public int results_remaining { get; set; }

            [JsonProperty(PropertyName = "matches")]
            public List<Match> Matches { get; set; }
        }

        public class Root
        {
            public Result result { get; set; }
        }

    }
}
