using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
            Dictionary<string, int> team_Pos = new Dictionary<string, int>();
            string convertedData = Convert.ToString(data, 2).PadLeft(8, '0');
            char[] convertedDataArray = convertedData.ToCharArray();

            team_Pos.Add("Team", convertedDataArray[0] - 48);

            for (int i = 7; i > 4; i--)
            {
                threelastbites += convertedDataArray[i];
            }

            int playerpos = Convert.ToInt32(Convert.ToSByte(threelastbites));
            team_Pos.Add("Position", playerpos);

            // Team : Radiant - 1, Dire - 0;
            return team_Pos;
        }

        public static string PlayerStatusDecipher(int profileState)
        {
            Dictionary<int, string> playerStatus = new Dictionary<int, string>();
            playerStatus.Add(0, "Offline");
            playerStatus.Add(1, "Online");
            playerStatus.Add(2, "Busy");
            playerStatus.Add(3, "Away");
            playerStatus.Add(4, "Snooze");
            playerStatus.Add(5, "Looking for trade");
            playerStatus.Add(6, "Looking for play");
            return playerStatus[profileState];
        }

        public static int WinChecker(Dictionary<string, int> playerSlot, bool radiantWins)
        {
            int Wins = 0;
            switch (playerSlot["Team"])
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

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }

            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }

        public static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true; 
        }

        public static decimal ConvertToSteamID32(decimal steamID64) => steamID64 - 76561197960265728;

        public static decimal ConvertToSteamID64(decimal steamID32) => steamID32 + 76561197960265728;

    }
}
