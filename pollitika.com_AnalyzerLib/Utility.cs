using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ScrapySharp.Html.Forms;
using ScrapySharp.Network;

namespace pollitika.com_AnalyzerLib
{
    public class Utility
    {
        public static ScrapingBrowser GetLoggedBrowser()
        {
            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true; // Browser has many settings you can access in setup
            Browser.AllowMetaRedirect = true;
            Browser.Encoding = Encoding.UTF8;

            //go to the home page
            WebPage PageResult = Browser.NavigateToPage(new Uri("http://www.pollitika.com"));

            PageWebForm form = PageResult.FindFormById("user-login-form");
            // assign values to the form fields
            form["name"] = "Liberty Valance";
            form["pass"] = "economist0";
            form.Method = HttpVerb.Post;
            WebPage resultsPage = form.Submit();

            return Browser;
        }
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
