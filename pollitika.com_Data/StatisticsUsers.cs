using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pollitika.com_Analyzer;

namespace pollitika.com_Data
{
    public class StatisticsUsers
    {
        public static void GetUsersWithMostPosts(int numUsers, ModelRepository inRepo)
        {
            List<User> list = inRepo._dataStore.Users.OrderByDescending(p => p.PostsByUser.Count).Take(numUsers).ToList();

            Console.WriteLine("Users with most posts:");
            foreach (var user in list)
                Console.WriteLine("User {0,-18}   - posts {1}", user.NameHtml, user.PostsByUser.Count);
            Console.WriteLine("");
        }
        public static void GetUsersWithMostComments(int numUsers, ModelRepository inRepo)
        {
            List<User> list = inRepo._dataStore.Users.OrderByDescending(p => p.CommentsByUser.Count).Take(numUsers).ToList();

            Console.WriteLine("Users with most comments:");
            foreach (var user in list)
                Console.WriteLine("User {0,-18}   - comments {1}", user.NameHtml, user.CommentsByUser.Count);
            Console.WriteLine("");
        }
        public static void GetUsersWhoGaveMostVotes(int numUsers, ModelRepository inRepo)
        {
            List<User> list = inRepo._dataStore.Users.OrderByDescending(p => p.VotesByUser.Count).Take(numUsers).ToList();

            Console.WriteLine("Users who gave most votes:");
            foreach (var user in list)
                Console.WriteLine("User {0,-18}   - num.comm {1}", user.NameHtml, user.VotesByUser.Count);
            Console.WriteLine("");
        }
    }
}
