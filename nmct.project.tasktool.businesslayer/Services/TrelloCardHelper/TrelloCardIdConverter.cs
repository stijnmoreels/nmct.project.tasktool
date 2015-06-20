using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.businesslayer.Services.TrelloCardHelper
{
    // -------------------------------------------------------------------------------------------------------------------------
    // Stijn Moreels - 04.06.2015
    // 
    // Used to convert the CardId to the Creation Date of the Card
    // -------------------------------------------------------------------------------------------------------------------------
    public class TrelloCardIdConverter
    {
        public static DateTime GetDatetimeFromCardId(string cardId)
        {
            string hexValue = cardId.Substring(0, 8);
            int timestamp = Convert.ToInt32(hexValue, 16);
            return FromUnixTime(timestamp);
        }

        // Thanks to LukeH (Stackoverflow)
        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0);
            return epoch.AddSeconds(unixTime);
        }
    }
}
