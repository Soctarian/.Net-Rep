using System;
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

    }
}
