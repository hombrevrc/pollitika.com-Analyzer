using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace pollitika.com_Analyzer
{
    public class ScrappedComment
    {
        private int _id;
        private string _text;
        private string _authorNick;
        private DateTime _datePosted;

        private Comment _parentComment;     // if null, then it is first level comment (in the first level below post)
        private List<Comment> _childComments;

        private List<ScrappedVote> _listVotes;
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
            List<HtmlNode> firstLevelComments = comments.ChildNodes.Where(x => x.Id.StartsWith("comment")).ToList();



            return listComments;
        }
    }
}
