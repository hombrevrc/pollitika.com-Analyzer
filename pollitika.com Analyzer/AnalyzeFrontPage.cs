using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace pollitika.com_Analyzer
{
    public class AnalyzeFrontPage
    {
        // return list of URLs of posts from the front page
        private static List<string> GetPostLinksFromFrontPage(int pageIndex)
        {
            List<string> retList = new List<string>();

            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true; // Browser has settings you can access in setup
            Browser.AllowMetaRedirect = true;
            Browser.Encoding = Encoding.UTF8;

            string pageUrl = pageIndex <= 0 ? "http://www.pollitika.com/node" : "http://pollitika.com/node?page=" + pageIndex.ToString();

            WebPage PageResult = Browser.NavigateToPage(new Uri(pageUrl));

            List<string> Names = new List<string>();
            var Table = PageResult.Html.CssSelect(".node");

            foreach (var post in Table)
            {
                var a = post.CssSelect(".first");

                foreach (var row in post.SelectNodes("h1"))
                {
                    string title = row.InnerText;
                    string html = row.InnerHtml;

                    int start = html.IndexOf("href=\"");
                    int end = html.IndexOf("\">");

                    string href = html.Substring(start + 6, end - start - 6);

                    retList.Add(href);
                    Console.WriteLine(title + " - " + href);
                }
            }

            return retList;
        }
    }
}
