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
                AnalyzePosts.AnalyzePost(href);
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

            int numComments = AnalyzePosts.GetPostCommentsNum(main);

            HtmlNode comments = main.Descendants().SingleOrDefault(x => x.Id == "comments");
            List<HtmlNode> listComment = comments.ChildNodes.Where( x => x.Id.StartsWith("comment")).ToList();

            int b = 3;
        }

 
    }
}
