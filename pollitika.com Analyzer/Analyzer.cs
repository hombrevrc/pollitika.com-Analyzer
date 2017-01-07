using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pollitika.com_Analyzer
{
    public class Analyzer
    {
        public Post AnalyzePost(string inPostUrl)
        {
            Post newPost = new Post();
            
            return newPost;
        }

        private List<Vote> GetPostVotes()
        {
            List<Vote> listVotes = new List<Vote>();

            return listVotes;
        }

        private List<Comment> GetPostComments()
        {
            List<Comment> listComments = new List<Comment>();

            return listComments;
        }

        private List<Vote> GetCommentVotes()
        {
            List<Vote> listVotes = new List<Vote>();

            return listVotes;
        }


    }
}