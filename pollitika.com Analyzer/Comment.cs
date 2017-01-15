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

        private List<Vote> _listVotes;
    }
}