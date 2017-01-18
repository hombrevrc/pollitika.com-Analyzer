using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pollitika.com_Analyzer
{
    public class Post
    {
        private int _id;                    // Id of the node
        private string _hrefLink;

        private string      _title;
        private User        _author;
        private DateTime    _datePosted;
        private string      _text;
        private int         _numCommentsScrapped;
        private bool        _isOnFrontPage;

        private List<Comment> _listComments;

        private string      _votesLink;
        private List<Vote>  _listVotes;

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

        public int GetNumberOfVotes()
        {
            return _listVotes.Count;
        }
        public int GetNumberOfComments()
        {
            return _listVotes.Count;
        }
    }
}
