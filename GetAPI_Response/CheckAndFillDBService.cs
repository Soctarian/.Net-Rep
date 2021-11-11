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
          /*  try 
            {*/
                var infoDictionary = new Dictionary<string, string>();
                var UserList = new List<long>();
                int matches = 0;
                GetMatchHistory.Root deserializedData;
                IEnumerable<GetMatchHistory.Match> deserializedMatchesList;

                using (var db = new UserContext())
                {
                    foreach(var member in db.Users)
                    {
                        deserializedData = GetUrls.GetMatchHistoryUrl(Deciphers.ConvertToSteamID32(Convert.ToDecimal(member.SteamID)));
                        if (deserializedData.result.Matches == null) continue;

                        if (member.LastTimeMatchesRefreshed == 0)
                        {
                            foreach(var match in deserializedData.result.Matches)
                            {
                                Matches newMatch = new Matches
                                {
                                    SteamID = member.SteamID,
                                    MatchID = Convert.ToInt64(match.MatchId),
                                    StartTime = match.start_time,

                                };
                                db.Matches.Add(newMatch);
                            }
                            matches += deserializedData.result.Matches.Count();
                            member.LastTimeMatchesRefreshed = DateTimeOffset.Now.ToUnixTimeSeconds();
                        }
                        else
                        {
                            deserializedMatchesList = deserializedData.result.Matches
                                .Where(match => match.start_time < DateTimeOffset.Now.ToUnixTimeSeconds() &&
                                match.start_time > db.Users.Find(member.SteamID).LastTimeMatchesRefreshed);
                            foreach (var match in deserializedMatchesList)
                            {
                                Matches newMatch = new Matches
                                {
                                    SteamID = member.SteamID,
                                    MatchID = Convert.ToInt64(match.MatchId),
                                    StartTime = match.start_time,

                                };
                                db.Matches.Add(newMatch);
                            }
                            matches += deserializedMatchesList.Count();
                            member.LastTimeMatchesRefreshed = DateTimeOffset.Now.ToUnixTimeSeconds();
                        }
                    }
                    db.SaveChanges();
                    infoDictionary.Add("Update accounts count", Convert.ToString(db.Users.Count()));
                    infoDictionary.Add("Added mathces count", Convert.ToString(matches));
                    
                }
                infoDictionary.Add("Result", "Success");
                    return infoDictionary;
        /*    }
            catch (Exception ex)
            {
                var infoDictionary = new Dictionary<string, string>();
                Console.WriteLine($"Error : {ex.Message}");
                infoDictionary.Add("Result", "Failed");
                return infoDictionary;
            }*/
        }
    }
}
