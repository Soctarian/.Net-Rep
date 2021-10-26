﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Controllers
{
    public class Deciphers
    {



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
        public static decimal ConvertToSteamID32(decimal SteamID64) => SteamID64 - 76561197960265728;
        public static decimal ConvertToSteamID64(decimal SteamID32) => SteamID32 + 76561197960265728;

    }
}
