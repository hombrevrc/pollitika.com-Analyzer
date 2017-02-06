﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pollitika.com_Model;

namespace pollitika.com_Analyzer
{
    [Serializable]
    public class User
    {
        private string _name;
        private string _nameHtml;
        private string _memberSince;
        private readonly List<Post>     _listPostsByUser = new List<Post>();
        private readonly List<Comment>  _listCommentsByUser = new List<Comment>();
        private List<Vote>     _listVotesByUser = new List<Vote>();

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string NameHtml
        {
            get { return _nameHtml; }
            set { _nameHtml = value; }
        }

        public string MemberSince
        {
            get { return _memberSince; }
            set { _memberSince = value; }
        }

        public List<Post> PostsByUser
        {
            get { return _listPostsByUser; }
        }

        public List<Comment> CommentsByUser
        {
            get { return _listCommentsByUser; }
        }

        public List<Vote> VotesByUser
        {
            get
            {
                if( _listVotesByUser == null )
                    _listVotesByUser = new List<Vote>();

                return _listVotesByUser;
            }
        }

        public int GetNumberOfNegativeVotes()
        {
            int sum = 0;
            foreach (var vote in _listVotesByUser)
                if (vote.UpOrDown == -1)
                    sum++;

            return sum;

        }
        public int GetNumberOfVotesOnPosts(IModelRepository inRepo)        // TODO - mroati će se proslijediti lista svih postova
        {
            return 0;
        }
        public int GetNumberOfVotesOnComments(IModelRepository inRepo)
        {
            return 0;
        }

        public double GetAverageVotesPerPost()
        {
            double sum = _listPostsByUser.Aggregate(0.0, (current, post) => current + post.GetNumberOfVotes());

            return sum / _listPostsByUser.Count;
        }
        public double GetAverageVotesPerComment()
        {
            double sum = _listCommentsByUser.Aggregate(0.0, (current, comment) => current + comment.Votes.Count);

            return sum / _listCommentsByUser.Count;
        }

        public void AddPostToList(Post inPost)
        {
            if (PostsByUser.Count(p => p.Id == inPost.Id) == 0)
                PostsByUser.Add(inPost);
        }
        public void AddCommentToList(Comment inComment)
        {
            if (CommentsByUser.Count(p => p.Id == inComment.Id) == 0)
                CommentsByUser.Add(inComment);
        }
        public void AddVoteToList(Vote inVote)
        {
            if (VotesByUser.Count(p => p.VoteOnNodeId == inVote.VoteOnNodeId) == 0)
                VotesByUser.Add(inVote);
        }
    }
}
