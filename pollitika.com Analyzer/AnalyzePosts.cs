using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using HtmlAgilityPack;
using pollitika.com_Model;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace pollitika.com_Analyzer
{
    public class AnalyzePosts
    {
        public static Post AnalyzePost(string inPostUrl, IModelRepository inRepo, bool isOnFrontPage, bool inFetchCommentsVotes = false)
        {
            Post newPost = new Post();
            newPost.HrefLink = inPostUrl;
            newPost.IsOnFrontPage = isOnFrontPage;

            StringBuilder output = new StringBuilder();
            output.AppendFormat("Post - {0,-90}", inPostUrl);

            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true; // Browser has settings you can access in setup
            Browser.AllowMetaRedirect = true;
            Browser.Encoding = Encoding.UTF8;

            HtmlWeb      htmlWeb = new HtmlWeb();
            HtmlDocument htmlDocument = htmlWeb.Load(inPostUrl);
            HtmlNode     mainContent = htmlDocument.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            int nodeId;
            string votesLink;
            if (AnalyzePosts.ScrapePostID(mainContent, out nodeId, out votesLink))
            {
                newPost.Id = nodeId;
                newPost.VotesLink = votesLink;
            }

            // check for Post ID already in the repo
            if (inRepo.PostAlreadyExists(newPost.Id))
            {
                Console.WriteLine("  WARNING - Post with ID {0} already in the database", newPost.Id);

                return null;
            }
            output.AppendFormat(" ID - {0}", newPost.Id);

            var titleHtml = mainContent.Descendants().Single(n => n.GetAttributeValue("class", "").Equals("node")).Descendants("h1").ToList();
            newPost.Title = titleHtml[0].InnerText;
            newPost.DatePosted = ScrapePostDate(mainContent);

            //Console.WriteLine("  Title    - {0}", newPost.Title);
            output.AppendFormat(" Date - {0,-20}", newPost.DatePosted);

            string author, authorHtml;
            AnalyzePosts.ScrapePostAuthor(htmlDocument, out author, out authorHtml);

            output.AppendFormat(" Username - {0,-18}", author);
            //Console.WriteLine("  Nick     - {0}", authorHtml);

            // check if user exists, add him if not
            User user = inRepo.GetUserByName(author);
            if (user == null)
            {
                user = new User{Name = author, NameHtml = authorHtml};
                inRepo.AddUser(user);
            }

            newPost.Author = user;

            newPost.NumCommentsScrapped = AnalyzeComments.ScrapePostCommentsNum(mainContent);
            if (newPost.NumCommentsScrapped < 0)
                Console.WriteLine("ERROR - scrapping number of comments");

            output.AppendFormat("  Num.comm - {0}", newPost.NumCommentsScrapped);

            if (newPost.Id > 0)
            {
                newPost.Votes = AnalyzeVotes.ScrapeListVotesForNode(newPost.Id, "node", inRepo);
            }

            output.AppendFormat("  Votes    - {0}", newPost.Votes.Count);

            Console.WriteLine(output.ToString());

            newPost.Comments = AnalyzeComments.ScrapePostComments(mainContent, inPostUrl, inRepo, inFetchCommentsVotes);

            //Console.WriteLine("  Comments - {0}", newPost.Comments.Count);

            return newPost;
        }

        public static bool ScrapePostID(HtmlNode nodeContentMain, out int outNodeId, out string votesLink)
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
        public static string ScrapePostAuthor(HtmlDocument htmlDocument, out string authorName, out string authorHtmlName)
        {
            HtmlNode userDetails = htmlDocument.DocumentNode.Descendants().Single(n => n.GetAttributeValue("class", "").Equals("breadcrumb"));

            string name = userDetails.LastChild.InnerHtml;

            if (name.Length > 12)
            {
                authorName = name.Substring(0, name.Length - 12);

                string html = userDetails.InnerHtml;
                int ind1 = html.IndexOf("/blog/");
                int ind2 = html.IndexOf("\">", ind1);

                authorHtmlName = html.Substring(ind1 + 6, ind2 - ind1 - 6);

                string href = userDetails.InnerHtml; // <a href="/node/15397/who_voted">Tko je glasao</a>
            }
            else
            {
                var userDetails1 = htmlDocument.DocumentNode.Descendants().Where(n => n.GetAttributeValue("class", "").Equals("article-meta article-meta-top")).ToList();

                var str = userDetails1[0].InnerText;
                int startInd = 14;
                int endInd = str.IndexOf('-');

                authorName = str.Substring(startInd - 1, endInd - startInd);
                authorHtmlName = str.Substring(startInd - 1, endInd - startInd);

                // "\n    Dnevnik mrak - Čet, 04/08/2011 - 15:05\n    Glasujte \n    Komentari 83 dodaj komentar\n  "


            }

            return "";
        }
        public static DateTime ScrapePostDate(HtmlNode nodeContentMain)
        {
            var commonPosts = nodeContentMain.Descendants().Single(n => n.GetAttributeValue("class", "").Equals("article-meta article-meta-top")).Descendants("li").ToList();

            // Date formats
            // Text - "Dnevnik žaki - Pon, 14/11/2016 - 18:07"
            // InnerHtml - "<span class=\"meta-title\">Dnevnik</span> žaki - Pon, 14/11/2016 - 18:07"

            DateTime retDate = Utility.ExtractDateTime(commonPosts[0].InnerText);

            return retDate;
        }
 



    }
}