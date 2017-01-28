using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pollitika.com_Analyzer;

namespace pollitika.com_Model
{
    public interface IModelRepository
    {
        void AddPost(Post newPost);
        void AddUser(User newUser);

        User GetUserByNick(string inNick);
    }
}
