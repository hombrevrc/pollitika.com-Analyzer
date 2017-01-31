using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pollitika.com_Analyzer
{
    public class Comment
    {
        private int _id;
        private string  _text;
        private User    _author;
        private DateTime _datePosted;

        private Comment _parentComment;     // if null, then it is first level comment (in the first level below post)
        private List<Comment> _childComments;

        private int _numScrappedVotes;          // scrapped from page
        private List<Vote> _listVotes = new List<Vote>();

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public User Author
        {
            get { return _author; }
            set { _author = value; }
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

        public int NumScrappedVotes
        {
            get { return _numScrappedVotes; }
            set { _numScrappedVotes = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
    }
}