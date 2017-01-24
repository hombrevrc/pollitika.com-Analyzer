using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace pollitika.com_Analyzer
{
    public class AnalyzePosts
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

        public static Post AnalyzePost(string inPostUrl)
        {
            Post newPost = new Post();
            newPost.HrefLink = inPostUrl;

            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true; // Browser has settings you can access in setup
            Browser.AllowMetaRedirect = true;
            Browser.Encoding = Encoding.UTF8;

            //WebPage PageResult = Browser.NavigateToPage(new Uri(pageUrl));

            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument htmlDocument = htmlWeb.Load(inPostUrl);
            HtmlNode mainContent = htmlDocument.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            int nodeId;
            string votesLink;
            if (AnalyzePosts.GetPostID(mainContent, out nodeId, out votesLink))
            {
                newPost.Id = nodeId;
                newPost.VotesLink = votesLink;
            }

            newPost.DatePosted = GetPostDate(mainContent);

            HtmlNode userDetails = htmlDocument.DocumentNode.Descendants().Single(n => n.GetAttributeValue("class", "").Equals("breadcrumb"));
            string author, authorHtml;
            AnalyzePosts.GetPostAuthor(userDetails, out author, out authorHtml);

            newPost.NumCommentsScrapped = AnalyzePosts.GetPostCommentsNum(mainContent);
            if (newPost.NumCommentsScrapped < 0)
                Console.WriteLine("Error scrapping number of comments");

            List<Vote> lVote = AnalyzeVotes.ScrapeListVotesForPost(newPost.Id);

            return newPost;
        }

        public static bool GetPostID(HtmlNode nodeContentMain, out int outNodeId, out string votesLink)
        {
            List<HtmlNode> commonPosts = nodeContentMain.Descendants().Single(n => n.GetAttributeValue("class", "").Equals("tabs primary")).Descendants("li").ToList();
            //List<HtmlNode> commonPosts = nodeContentMain.Descendants().Where(n => n.GetAttributeValue("class", "").Equals("tabs primary")).Single().Descendants("li").ToList();

            string href = commonPosts[1].InnerHtml;   // <a href="/node/15397/who_voted">Tko je glasao</a>
            int ind1 = href.IndexOf("href=");
            int ind2 = href.IndexOf("Tko je glasao");

            string nodeId = href.Substring(ind1 + 12, ind2 - ind1 - 24);
            outNodeId = Convert.ToInt32(nodeId);

            votesLink = href.Substring(ind1 + 6, ind2 - ind1 - 8);

            return true;
        }
        public static string GetPostAuthor(HtmlNode nodeContentMain, out string authorName, out string authorHtmlName)
        {
            string name = nodeContentMain.LastChild.InnerHtml;
            authorName = name.Substring(0, name.Length - 12);

            string html = nodeContentMain.InnerHtml;
            int ind1 = html.IndexOf("/blog/");
            int ind2 = html.IndexOf("\">", ind1);

            authorHtmlName = html.Substring(ind1+6, ind2 - ind1 - 6);

            string href = nodeContentMain.InnerHtml;   // <a href="/node/15397/who_voted">Tko je glasao</a>
            //
            return "";
        }
        public static DateTime GetPostDate(HtmlNode nodeContentMain)
        {
            var commonPosts = nodeContentMain.Descendants().Single(n => n.GetAttributeValue("class", "").Equals("article-meta article-meta-top")).Descendants("li").ToList();

            // extracting date
            DateTime dt1 = new DateTime();
            var regexDate = new Regex(@"\b\d{2}/\d{2}/\d{4}\b");
            foreach (Match m in regexDate.Matches(commonPosts[0].InnerText))
            {
                if (DateTime.TryParseExact(m.Value, "dd/MM/yyyy", null, DateTimeStyles.None, out dt1))
                {
                    break;
                }
            }
            // extracting time
            int hh=0, mm=0;
            var regexTime = new Regex(@"\b\d{2}:\d{2}\b");
            foreach (Match m in regexTime.Matches(commonPosts[0].InnerText))
            {
                var values = m.Value.Split(':');
                hh = Convert.ToInt32(values[0]);
                mm = Convert.ToInt32(values[1]);
            }

            DateTime retDate = new DateTime(dt1.Year, dt1.Month, dt1.Day, hh, mm, 0);

            // Text - "Dnevnik žaki - Pon, 14/11/2016 - 18:07"
            // InnerHtml - "<span class=\"meta-title\">Dnevnik</span> žaki - Pon, 14/11/2016 - 18:07"

            return retDate;
        }
        public static int GetPostCommentsNum(HtmlNode nodeContentMain)
        {
            List<HtmlNode> commonPosts = nodeContentMain.Descendants().Where(n => n.GetAttributeValue("class", "").Equals("broj-komentara")).ToList();

            int numComments = -1;
            if (commonPosts[0] != null)
            {
                numComments = Convert.ToInt32(commonPosts[0].InnerText);
            }

            return numComments;
        }

        public static List<Comment> GetPostComments(HtmlNode mainNode)
        {
            List<Comment> listComments = new List<Comment>();

            HtmlNode comments = mainNode.Descendants().SingleOrDefault(x => x.Id == "comments");
            List<HtmlNode> firstLevelComments = comments.ChildNodes.Where(x => x.Id.StartsWith("comment")).ToList();



            return listComments;
        }

        private List<Vote> GetPostVotes()
        {
            List<Vote> listVotes = new List<Vote>();

            return listVotes;
        }

        private List<Comment> GetPostComments()
        {
            List<Comment> listComments = new List<Comment>();

            return listComments;
        }

        private List<Vote> GetCommentVotes()
        {
            List<Vote> listVotes = new List<Vote>();

            return listVotes;
        }


    }
}