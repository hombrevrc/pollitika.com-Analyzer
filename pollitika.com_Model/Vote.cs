﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pollitika.com_Model
{
    [Serializable]
    public class Vote
    {
        private User _byUser;
        private int _voteOnNodeID;
        private DateTime _datePosted;
        private int _upOrDown;
        private User _voteForUser = null;

        public User ByUser
        {
            get { return _byUser; }
            set { _byUser = value; }
        }

        public DateTime DatePosted
        {
            get { return _datePosted; }
            set { _datePosted = value; }
        }

        public int UpOrDown
        {
            get { return _upOrDown; }
            set { _upOrDown = value; }
        }

        public int VoteOnNodeId
        {
            get { return _voteOnNodeID; }
            set { _voteOnNodeID = value; }
        }

        public User VoteForUser
        {
            get { return _voteForUser; }
            set { _voteForUser = value; }
        }
    }
}