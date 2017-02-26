using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pollitika.com_Model
{
    public interface IModelRepository
    {
        void AddPost(Post newPost);
        void AddUser(User newUser);

        bool PostAlreadyExists(int inPostID);
        User GetUserByName(string inName);
        User GetUserByNick(string inNick);
    }
}
