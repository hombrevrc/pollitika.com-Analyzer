using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace pollitika.com_Analyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 1; i++)
            {
                //Console.WriteLine("DOING PAGE - {0}", i);
                //AnalyzeFrontPage(i);

                string href = "http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija";
                AnalyzePost(href);
            }
        }

        private static void AnalyzeFrontPage(int pageIndex)
        {
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

                    Console.WriteLine(title + " - " + href);
                }
            }
        }

        private static void AnalyzePost(string pageUrl)
        {
            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true; // Browser has settings you can access in setup
            Browser.AllowMetaRedirect = true;
            Browser.Encoding = Encoding.UTF8;

            //WebPage PageResult = Browser.NavigateToPage(new Uri(pageUrl));

            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument htmlDocument = htmlWeb.Load(pageUrl);
            HtmlNode main = htmlDocument.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            int numComments = Analyzer.GetPostCommentsNum(main);

            HtmlNode comments = main.Descendants().SingleOrDefault(x => x.Id == "comments");
            List<HtmlNode> listComment = comments.ChildNodes.Where( x => x.Id.StartsWith("comment")).ToList();

            int b = 3;
        }

 
    }
}
