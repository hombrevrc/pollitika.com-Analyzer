using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pollitika.com_Analyzer
{
    [Serializable]
    public class Post
    {
        private int _id = -1;                    // Id of the node
        private string _hrefLink;

        private string      _title;
        private User        _author;
        private DateTime    _datePosted;
        private string      _text;
        private int         _numCommentsScrapped;
        private bool        _isOnFrontPage;

        private List<Comment> _listComments = new List<Comment>();

        private string      _votesLink;
        private List<Vote>  _listVotes = new List<Vote>();

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string HrefLink
        {
            get { return _hrefLink; }
            set { _hrefLink = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public int NumCommentsScrapped
        {
            get { return _numCommentsScrapped; }
            set { _numCommentsScrapped = value; }
        }

        public User Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string VotesLink
        {
            get { return _votesLink; }
            set { _votesLink = value; }
        }

        public DateTime DatePosted
        {
            get { return _datePosted; }
            set { _datePosted = value; }
        }

        public List<Vote> Votes
        {
            get { return _listVotes; }
            set { _listVotes = value; }
        }

        public List<Comment> Comments
        {
            get { return _listComments; }
            set { _listComments = value; }
        }

        public int GetNumberOfVotes()
        {
            return Votes.Count;
        }
        public int GetNumberOfComments()
        {
            return Comments.Count;
        }
    }
}
