using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using ScrapySharp.Network;

namespace pollitika.com_Analyzer
{
    public class Analyzer
    {
        public Post AnalyzePost(string inPostUrl)
        {
            Post newPost = new Post();

            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true; // Browser has settings you can access in setup
            Browser.AllowMetaRedirect = true;
            Browser.Encoding = Encoding.UTF8;

            //WebPage PageResult = Browser.NavigateToPage(new Uri(pageUrl));

            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument htmlDocument = htmlWeb.Load(inPostUrl);
            HtmlNode main = htmlDocument.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            int numComments = Analyzer.GetPostCommentsNum(main);

            HtmlNode comments = main.Descendants().SingleOrDefault(x => x.Id == "comments");
            List<HtmlNode> listComment = comments.ChildNodes.Where(x => x.Id.StartsWith("comment")).ToList();

            return newPost;
        }

        public static int GetPostCommentsNum(HtmlNode mainNode)
        {
            List<HtmlNode> commonPosts = mainNode.Descendants().Where(n => n.GetAttributeValue("class", "").Equals("broj-komentara")).ToList();

            int numComments = -1;
            if (commonPosts[0] != null)
            {
                numComments = Convert.ToInt32(commonPosts[0].InnerText);
            }

            return numComments;
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