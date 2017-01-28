using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace pollitika.com_Analyzer
{
    public class ScrappedComment
    {
        public int _id;                 // node ID
        public string _text;
        public string _authorNick;
        public DateTime _datePosted;

        public int _parentCommentID;            // if 0, then it is first level comment (in the first level below post)
        public List<int> _childCommentsIDs;

        private int _numScrappedVotes;          // scrapped from page
        public List<ScrappedVote> _listVotes;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public DateTime DatePosted
        {
            get { return _datePosted; }
            set { _datePosted = value; }
        }

        public string AuthorNick
        {
            get { return _authorNick; }
            set { _authorNick = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public int NumScrappedVotes
        {
            get { return _numScrappedVotes; }
            set { _numScrappedVotes = value; }
        }
    }

    public class AnalyzeComments
    {
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

        public static List<ScrappedComment> GetPostComments(HtmlNode mainNode)
        {
            List<ScrappedComment> listComments = new List<ScrappedComment>();

            HtmlNode comments = mainNode.Descendants().SingleOrDefault(x => x.Id == "comments");
            List<HtmlNode> allComments = comments.Descendants().Where(x => x.Id.StartsWith("comment-content")).ToList();

            foreach (var comment in allComments)
            {
                ScrappedComment newComment = new ScrappedComment();

                //comment.ChildNodes[1] ima "\n    Skviki &mdash; Pon, 28/11/2016 - 16:16.  
                string  strNameDate = comment.ChildNodes[1].InnerText;
                int     mdashPos = strNameDate.IndexOf("&mdash");
                string  name = strNameDate.Substring(2, mdashPos - 2);

                newComment.AuthorNick = name.Trim();

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
