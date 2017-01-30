using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using pollitika.com_Model;

namespace pollitika.com_Analyzer
{
 public class AnalyzeComments
    {
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

        public static List<Comment> ScrapePostComments(HtmlNode mainNode, IModelRepository inRepo)
        {
            List<Comment> listComments = new List<Comment>();

            HtmlNode comments = mainNode.Descendants().SingleOrDefault(x => x.Id == "comments");
            List<HtmlNode> allComments = comments.Descendants().Where(x => x.Id.StartsWith("comment-content")).ToList();

            // TODO - ishendlati kad ima više stranica s komentarima

            foreach (var comment in allComments)
            {
                Comment newComment = new Comment();

                //comment.ChildNodes[1] has "\n    Skviki &mdash; Pon, 28/11/2016 - 16:16.  
                string  strNameDate = comment.ChildNodes[1].InnerText;
                int     mdashPos = strNameDate.IndexOf("&mdash");
                string  name = strNameDate.Substring(2, mdashPos - 2);

                string authorNick = name.Trim();
                // check if user exists, add him if not
                User user = inRepo.GetUserByName(authorNick);
                if (user == null)
                {
                    user = new User { Name = authorNick, NameHtml = authorNick };
                    inRepo.AddUser(user);
                }
                newComment.Author = user;

                int     lastCommaPos = strNameDate.LastIndexOf(',');
                string  date = strNameDate.Substring(lastCommaPos+1, strNameDate.Length - lastCommaPos-1);

                var numVotes = comment.Descendants().Where(n => n.GetAttributeValue("class", "").Equals("total-votes-plain")).ToList();
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
                    Console.WriteLine("ERROR in getting comment ID");
                }

                listComments.Add(newComment);
            }

            // and now we have to fetch list of votes for each one
            foreach (var comm in listComments)
            {
                comm.ListVotes = AnalyzeVotes.ScrapeListVotesForNode(comm.Id, inRepo);
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
