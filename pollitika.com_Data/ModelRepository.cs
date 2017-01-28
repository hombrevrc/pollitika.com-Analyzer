using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pollitika.com_Analyzer;
using pollitika.com_Model;

namespace pollitika.com_Data
{
    public class ModelRepository : IModelRepository
    {
        private DataStore _dataStore = new DataStore();
         
        public void AddPost(Post newPost)
        {
            _dataStore.Posts.Add(newPost);
        }

        public void AddUser(User newUser)
        {
            _dataStore.Users.Add(newUser);
        }

        public User GetUserByNick(string inNick)
        {
            return _dataStore.Users.FirstOrDefault(p => p.NameHtml == inNick);
        }
    }
}
