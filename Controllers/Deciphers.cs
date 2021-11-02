using System;
using System.Collections.Generic;
using System.Text;

namespace Controllers
{
    public class Deciphers
    {

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public static Dictionary<string, int> PlayerSlotDecipher(int data)
        {
            string threelastbites = " ";
            Dictionary<string, int> Team_Pos = new Dictionary<string, int>();
            string ConvertedData = Convert.ToString(data, 2).PadLeft(8, '0');
            char[] ConvertedDataArray = ConvertedData.ToCharArray();

            Team_Pos.Add("Team", ConvertedDataArray[0] - 48);

            for (int i = 7; i > 4; i--) threelastbites += ConvertedDataArray[i];
            int playerpos = Convert.ToInt32(Convert.ToSByte(threelastbites));
            Team_Pos.Add("Position", playerpos);
            //Team : Radiant - 1, Dire - 0;
            return Team_Pos;
        }
        public static string PlayerStatusDecipher(int ProfileState)
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

        public static int WinChecker(Dictionary<string, int> PlayerSlot, bool radiantWins)
        {
            int Wins = 0;
            switch (PlayerSlot["Team"])
            {
                case 0:
                    Wins = radiantWins ? 1 : 0;
                    break;
                case 1:
                    Wins = radiantWins ? 0 : 1;
                    break;
            }
            return Wins; 
        }
        public static decimal ConvertToSteamID32(decimal SteamID64) => SteamID64 - 76561197960265728;
        public static decimal ConvertToSteamID64(decimal SteamID32) => SteamID32 + 76561197960265728;

    }
}
