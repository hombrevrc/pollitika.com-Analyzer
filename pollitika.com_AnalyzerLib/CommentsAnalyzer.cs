using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

using pollitika.com_Model;
using ScrapySharp.Network;

namespace pollitika.com_AnalyzerLib
{
 public class CommentsAnalyzer
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static int ScrapePostCommentsNum(HtmlNode nodeContentMain)
        {
            List<HtmlNode> commonPosts = nodeContentMain.Descendants().Where(n => n.GetAttributeValue("class", "").Equals("broj-komentara")).ToList();

            int numComments = -1;
            if (commonPosts[0] != null)
            {
                numComments = Convert.ToInt32(commonPosts[0].InnerText);
            }

            return numComments;
        }

        public static List<Comment> ScrapePostComments(HtmlNode mainNode, string inHref, IModelRepository inRepo, bool inFetchCommentsVotes, ScrapingBrowser Browser = null)
        {
            List<Comment> listComments = new List<Comment>();

            // first - check if we have multiple pages of comments
            // najprije, da vidimo da li je samo jedna stranica s glasovima ili ih ima više
            var itemlist = mainNode.Descendants().Where(n => n.GetAttributeValue("class", "").Equals("pager")).ToList();

            int pageCount = 0;
            if (itemlist.Count > 0)
            {
                string s = itemlist[0].LastChild.PreviousSibling.InnerHtml;

                int n1 = s.IndexOf("?page=");
                int n2 = s.IndexOf("\"", n1);
                string num = s.Substring(n1 + 6, n2 - n1 - 6);

                pageCount = Convert.ToInt32(num);
            }

            for (int i = 0; i <= pageCount; i++)
            {

                HtmlNode comments = mainNode.Descendants().SingleOrDefault(x => x.Id == "comments");

                if (comments == null)           // No comments?
                    return listComments;

                List<HtmlNode> allComments = comments.Descendants().Where(x => x.Id.StartsWith("comment-content")).ToList();

                foreach (var comment in allComments)
                {
                    Comment newComment = new Comment();

                    //comment.ChildNodes[1] has "\n    Skviki &mdash; Pon, 28/11/2016 - 16:16.  
                    string strNameDate = comment.ChildNodes[1].InnerText;
                    int mdashPos = strNameDate.IndexOf("&mdash");
                    string name = strNameDate.Substring(2, mdashPos - 2);
                    string authorName = name.Trim();

                    // let's see if we can get his html nick
                    string authorNick = "";
                    string str = comment.ChildNodes[1].InnerHtml;
                    int usrInd = str.IndexOf("/user/");
                    if (usrInd != -1)
                    {
                        int usrInd2 = str.IndexOf("title=", usrInd);
                        authorNick = str.Substring(usrInd + 6, usrInd2 - usrInd - 8);
                    }
                    // check if user exists, add him if not
                    User user = inRepo.GetUserByName(authorName);
                    if (user == null)
                    {
                        user = new User {Name = authorName, NameHtml = authorNick};
                        inRepo.AddUser(user);
                    }
                    newComment.Author = user;

                    int lastCommaPos = strNameDate.LastIndexOf(',');
                    string date = strNameDate.Substring(lastCommaPos + 1, strNameDate.Length - lastCommaPos - 1);

                    var numVotes =
                        comment.Descendants()
                            .Where(n => n.GetAttributeValue("class", "").Equals("total-votes-plain"))
                            .ToList();
                    string resultString = Regex.Match(numVotes[0].InnerText, @"-?\d+").Value;

                    newComment.NumScrappedVotes = Int32.Parse(resultString);

                    newComment.DatePosted = Utility.ExtractDateTime(date.Trim());

                    newComment.Text = comment.ChildNodes[3].InnerText;

                    string commentId = comment.Id;
                    int dashPos = commentId.LastIndexOf('-');
                    if (dashPos > 0)
                    {
                        string idValue = commentId.Substring(dashPos + 1, commentId.Length - dashPos - 1);

                        newComment.Id = Convert.ToInt32(idValue);
                    }
                    else
                    {
                        log.Error("ERROR in getting comment ID " + inHref);
                    }

                    listComments.Add(newComment);
                }

                // reinicijaliziramo učitani HTML za sljedeću stranicu
                if (i < pageCount)
                {
                    string href = inHref + "?page=" + (i + 1).ToString();

                    if (Browser != null)
                    {
                        WebPage PageResult = Browser.NavigateToPage(new Uri(href));
                        mainNode = PageResult.Html;

                        //log.Info(mainNode.InnerHtml);
                    }
                    else
                    {
                        HtmlWeb htmlWeb = new HtmlWeb();
                        HtmlDocument htmlDocument = htmlWeb.Load(href);
                        mainNode = htmlDocument.DocumentNode;
                    }
                }
            }

            // and now we have to fetch list of votes for each one
            if (inFetchCommentsVotes)
            {
                foreach (var comm in listComments)
                {
                    // ovaj if bi jako ubrzao stvar ... ali, što ukoliko je dobio dva plus i dva minus glasa, i rezultat je 0?
                    //if( comm.NumScrappedVotes != 0 )
                    comm.Votes = VotesAnalyzer.ScrapeListVotesForNode(comm.Id, comm.Author, "comment", inRepo, Browser);
                }
            }

            //List<HtmlNode> firstLevelComments = comments.ChildNodes.Where(x => x.Id.StartsWith("comment")).ToList();
            //foreach (var com1 in firstLevelComments)
            //{
            //    if( com1.Name == "div" )
            //        Console.WriteLine("DIV DIV DIV ******************************************\n" + com1.InnerHtml);
            //}


            return listComments;
        }
    }
}
