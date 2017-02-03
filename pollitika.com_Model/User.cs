using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
