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
                Console.WriteLine("User {0,-18}   - votes {1}", user.NameHtml, user.VotesByUser.Count);
            Console.WriteLine("");
        }
        public static void GetUsersWhoGaveMostNegativeVotes(int numUsers, ModelRepository inRepo)
        {
            List<User> list = inRepo._dataStore.Users.OrderByDescending(p => p.GetNumberOfNegativeVotes()).Take(numUsers).ToList();

            Console.WriteLine("Users who gave most negative votes:");
            foreach (var user in list)
                Console.WriteLine("User {0,-18}   - votes {1}", user.NameHtml, user.GetNumberOfNegativeVotes());
            Console.WriteLine("");
        }
        public static void GetUsersWhoGaveMostVotesOnPosts(int numUsers, ModelRepository inRepo)
        {
            List<User> list = inRepo._dataStore.Users.OrderByDescending(p => p.GetNumberOfVotesOnPosts(inRepo)).Take(numUsers).ToList();

            Console.WriteLine("Users who gave most votes on posts:");
            foreach (var user in list)
                Console.WriteLine("User {0,-18}   - votes {1}", user.NameHtml, user.GetNumberOfVotesOnPosts(inRepo));
            Console.WriteLine("");
        }
        public static void GetUsersWhoGaveMostVotesOnComments(int numUsers, ModelRepository inRepo)
        {
            List<User> list = inRepo._dataStore.Users.OrderByDescending(p => p.GetNumberOfVotesOnComments(inRepo)).Take(numUsers).ToList();

            Console.WriteLine("Users who gave most votes on comments:");
            foreach (var user in list)
                Console.WriteLine("User {0,-18}   - votes {1}", user.NameHtml, user.GetNumberOfVotesOnComments(inRepo));
            Console.WriteLine("");
        }

        public static void GetUsersWithBiggestAverageNumberOfVotesPerPost(int numUsers, ModelRepository inRepo)
        {
            List<User> list = inRepo._dataStore.Users.Where(p => p.PostsByUser.Count > 10).OrderByDescending(p => p.GetAverageVotesPerPost()).Take(numUsers).ToList();

            Console.WriteLine("Users with highest average of votes per post:");
            foreach (var user in list)
                Console.WriteLine("User {0,-18}   - votes {1}", user.NameHtml, user.GetAverageVotesPerPost());
            Console.WriteLine("");
        }

        public static void GetUsersWithBiggestAverageNumberOfVotesPerComment(int numUsers, ModelRepository inRepo)
        {
            List<User> list = inRepo._dataStore.Users.Where(p => p.CommentsByUser.Count > 10).OrderByDescending(p => p.GetAverageVotesPerComment()).Take(numUsers).ToList();

            Console.WriteLine("Users with highest average of votes per comment:");
            foreach (var user in list)
                Console.WriteLine("User {0,-18}   - votes {1}", user.NameHtml, user.GetAverageVotesPerComment());
            Console.WriteLine("");
        }

    }
}
