using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserClasses;
using Controllers;
using DeserializeObjects;
using System.Net.Http;

namespace GetAPI_Response
{
    public class CheckAndFillDBService
    {
        private HttpClient _httpClient;

        public CheckAndFillDBService(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<Dictionary<string, string>> CheckForNewMatches()
        {
            try
            {
                var infoDictionary = new Dictionary<string, string>();
                int matches = 0;
                GetMatchHistory.Root deserializedData;
                IEnumerable<GetMatchHistory.Match> deserializedMatchesList;

                using (var db = new UserContext())
                {

                    foreach (var member in db.Users)
                    {
                        deserializedData = GetUrls.GetMatchHistoryUrl(Deciphers.ConvertToSteamID32(Convert.ToDecimal(member.SteamID)));
                        if (deserializedData.result.Matches == null) continue;

                        if (member.LastTimeMatchesRefreshed == 0)
                        {
                            member.LastTimeMatchesRefreshed = DateTimeOffset.Now.ToUnixTimeSeconds();
                            matches += deserializedData.result.Matches.Count();
                            UpdateNewUserDB(deserializedData, member.SteamID);
                        }
                        else 
                        {
                            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(member.LastTimeMatchesRefreshed));
                            var lastCheckTimeV2 = dateTimeOffset.UtcDateTime.AddHours(2);

                            deserializedMatchesList = deserializedData.result.Matches
                                .Where(match => Deciphers.UnixTimeStampToDateTime(match.start_time) < DateTime.Now &&
                                Deciphers.UnixTimeStampToDateTime(match.start_time) > lastCheckTimeV2);


                            member.LastTimeMatchesRefreshed = DateTimeOffset.Now.ToUnixTimeSeconds();
                            matches += deserializedMatchesList.Count();
                            UpdateExistingUserDB(deserializedMatchesList, member.SteamID);
                        }
                    }


                    db.SaveChanges();
                    infoDictionary.Add("Update accounts count", Convert.ToString(db.Users.Count()));
                    infoDictionary.Add("Added mathces count", Convert.ToString(matches));

                }
                infoDictionary.Add("Result", "Success");
                return infoDictionary;
            }
            catch (Exception ex)
            {
                var infoDictionary = new Dictionary<string, string>();
                Console.WriteLine($"Error : {ex.Message}");
                infoDictionary.Add("Result", "Failed");
                return infoDictionary;
            }
        }

        private void UpdateNewUserDB(GetMatchHistory.Root matches, long SteamID)
        {
            using (var db = new UserContext())
            {
                foreach (var match in matches.result.Matches)
                {
                    Matches newMatch = new Matches
                    {
                        User_SteamID = SteamID,
                        MatchID = Convert.ToInt64(match.MatchId),
                        StartTime = match.start_time,
                    };
                    db.Matches.Add(newMatch);
                }
                db.SaveChanges();
            }
        }
        private void UpdateExistingUserDB(IEnumerable<GetMatchHistory.Match> matches, long SteamID)
        {
            using (var db = new UserContext())
            {
                foreach (var match in matches)
                {
                    Matches newMatch = new Matches
                    {
                        User_SteamID = SteamID,
                        MatchID = Convert.ToInt64(match.MatchId),
                        StartTime = match.start_time,
                    };
                    db.Matches.Add(newMatch);
                }
                db.SaveChanges();
            }
        }

    }
}
