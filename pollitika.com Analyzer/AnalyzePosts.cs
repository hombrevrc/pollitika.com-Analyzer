﻿using System;
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
        public static Post AnalyzePost(string inPostUrl, IModelRepository inRepo, bool inFetchCommentsVotes = true)
        {
            Post newPost = new Post();
            newPost.HrefLink = inPostUrl;

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

            newPost.DatePosted = ScrapePostDate(mainContent);

            string author, authorHtml;
            AnalyzePosts.ScrapePostAuthor(htmlDocument, out author, out authorHtml);

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
                Console.WriteLine("Error scrapping number of comments");

            if (newPost.Id > 0)
            {
                newPost.Votes = AnalyzeVotes.ScrapeListVotesForNode(newPost.Id, "node", inRepo);
            }

            newPost.Comments = AnalyzeComments.ScrapePostComments(mainContent, inPostUrl, inRepo, inFetchCommentsVotes);

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
            authorName = name.Substring(0, name.Length - 12);

            string html = userDetails.InnerHtml;
            int ind1 = html.IndexOf("/blog/");
            int ind2 = html.IndexOf("\">", ind1);

            authorHtmlName = html.Substring(ind1+6, ind2 - ind1 - 6);

            string href = userDetails.InnerHtml;   // <a href="/node/15397/who_voted">Tko je glasao</a>
            
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