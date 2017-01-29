using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace pollitika.com_Analyzer
{
    public class Utility
    {
        public static DateTime ExtractDateTime(string inStr)
        {
            // extracting date
            DateTime dt1 = new DateTime();
            var regexDate = new Regex(@"\b\d{2}/\d{2}/\d{4}\b");
            foreach (Match m in regexDate.Matches(inStr))
            {
                if (DateTime.TryParseExact(m.Value, "dd/MM/yyyy", null, DateTimeStyles.None, out dt1))
                {
                    break;
                }
            }
            // extracting time
            int hh = 0, mm = 0;
            var regexTime = new Regex(@"\b\d{2}:\d{2}\b");
            foreach (Match m in regexTime.Matches(inStr))
            {
                var values = m.Value.Split(':');
                hh = Convert.ToInt32(values[0]);
                mm = Convert.ToInt32(values[1]);
            }

            DateTime retDate = new DateTime(dt1.Year, dt1.Month, dt1.Day, hh, mm, 0);

            return retDate;
        }

        public static DateTime ExtractDateTime2(string inStr)
        {
            // extracting date
            DateTime dt1 = new DateTime();
            var regexDate = new Regex(@"\b\d{2}.\d{2}.\d{4}\b");
            foreach (Match m in regexDate.Matches(inStr))
            {
                if (DateTime.TryParseExact(m.Value, "dd.MM.yyyy", null, DateTimeStyles.None, out dt1))
                {
                    break;
                }
            }
            // extracting time
            int hh = 0, mm = 0;
            var regexTime = new Regex(@"\b\d{2}:\d{2}\b");
            foreach (Match m in regexTime.Matches(inStr))
            {
                var values = m.Value.Split(':');
                hh = Convert.ToInt32(values[0]);
                mm = Convert.ToInt32(values[1]);
            }

            DateTime retDate = new DateTime(dt1.Year, dt1.Month, dt1.Day, hh, mm, 0);

            return retDate;
        }
    }
}
