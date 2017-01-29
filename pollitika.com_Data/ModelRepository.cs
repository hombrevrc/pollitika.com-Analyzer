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
            // TODO - check for already existing Post ID
            _dataStore.Posts.Add(newPost);
        }

        public void AddUser(User newUser)
        {
            // TODO - check if user with name already exists
            _dataStore.Users.Add(newUser);
        }

        public User GetUserByName(string inName)
        {
            return _dataStore.Users.FirstOrDefault(p => p.Name == inName);
        }
        public User GetUserByNick(string inNick)
        {
            return _dataStore.Users.FirstOrDefault(p => p.NameHtml == inNick);
        }
    }
}
