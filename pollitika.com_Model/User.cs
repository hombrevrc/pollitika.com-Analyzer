using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pollitika.com_Model;

namespace pollitika.com_Model
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
        private List<Vote>     _listReceivedVotes = new List<Vote>();

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

        public List<Vote> ReceivedVotes
        {
            get { return _listReceivedVotes; }
        }

        public int GetNumberOfGivenNegativeVotes()
        {
            int sum = 0;
            foreach (var vote in _listVotesByUser)
                if (vote.UpOrDown == -1)
                    sum++;

            return sum;
        }
        public int GetNumberOfReceivedNegativeVotes()
        {
            int sum = 0;
            foreach (var vote in _listReceivedVotes)
                if (vote.UpOrDown == -1)
                    sum++;

            return sum;
        }
        public int GetNumberOfVotesOnPosts(IModelRepository inRepo)        // TODO - morati će se proslijediti lista svih postova
        {
            return 0;
        }
        public int GetNumberOfVotesOnComments(IModelRepository inRepo)
        {
            return 0;
        }

        public double GetAverageVotesPerPost()
        {
            double sum = 0.0;
            int cnt = 0;
            foreach (var post in _listPostsByUser)
            {
                if (post.Id > 1485 && post.DatePosted > new DateTime(2007, 9, 22))
                {
                    sum = sum + post.GetNumberOfVotes();
                    cnt++;
                }
            }

            return sum / cnt;
        }
        public double GetAverageCommentsPerPost()
        {
            double sum = 0.0;
            int cnt = 0;
            foreach (var post in _listPostsByUser)
            {
                if (post.Id > 1485 && post.DatePosted > new DateTime(2007, 9, 22))
                {
                    sum = sum + post.GetNumberOfComments();
                    cnt++;
                }
            }

            return sum / cnt;
        }

        public double GetAverageVotesPerComment()
        {
            double sum = _listCommentsByUser.Aggregate(0.0, (current, comment) => current + comment.Votes.Count);

            return sum / _listCommentsByUser.Count;
        }
        public int GetNumberOfPostsWithOverNVotes(int N)
        {
            return _listPostsByUser.Count(p => p.GetNumberOfVotes() >= N);
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
