using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pollitika.com_Analyzer;

namespace pollitika.com_Data
{
    [Serializable]
    public class DataStore
    {
        List<Post>    _listPosts = new List<Post>();
        List<Comment> _listComments = new List<Comment>(); 
        List<User>    _listUsers = new List<User>(); 
        List<Vote>    _listVotes = new List<Vote>();

        public List<Post> Posts
        {
            get { return _listPosts; }
        }

        public List<User> Users
        {
            get { return _listUsers; }
        }

        public List<Vote> Votes
        {
            get { return _listVotes; }
        }

        public List<Comment> Comments
        {
            get { return _listComments; }
        }

        public void Clear()
        {
            
        }
    }
}
