using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ClassLibrariesTest
{
    public class Class1
    {

        public class Player
        {
            public object account_id { get; set; }
            public int player_slot { get; set; }
            public int hero_id { get; set; }
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
            public List<Match> matches { get; set; }
        }

        public class Root
        {
            public Result result { get; set; }
        }

    }
}
