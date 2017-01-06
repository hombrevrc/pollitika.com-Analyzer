using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace pollitika.com_Analyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true; // Browser has settings you can access in setup
            Browser.AllowMetaRedirect = true;

            WebPage PageResult = Browser.NavigateToPage(new Uri("http://www.pollitika.com"));

            List<string> Names = new List<string>();
            var Table = PageResult.Html.CssSelect(".node");

            foreach (var post in Table)
            {
                Console.WriteLine("New");
                foreach (var row in post.SelectNodes("h1"))
                {
                    string s = row.InnerText;

                    Console.WriteLine(s);
                }
            }
        }
    }
}
