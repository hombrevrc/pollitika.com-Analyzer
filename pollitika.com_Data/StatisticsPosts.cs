using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pollitika.com_Analyzer;

namespace pollitika.com_Data
{
    public class StatisticsPosts
    {
        public static void GetSummary(ModelRepository inRepo)
        {
            Console.WriteLine("Total number of users   : {0}", inRepo._dataStore.Users.Count);
            Console.WriteLine("Total number of posts   : {0}", inRepo._dataStore.Posts.Count);
            Console.WriteLine("Total number of comments: {0}", inRepo._dataStore.Comments.Count);
            Console.WriteLine("Total number of votes   : {0}", inRepo._dataStore.Votes.Count);
        }

        public static void GetPostsWithMostNumberOfVotes(int numPosts, ModelRepository inRepo)
        {
            List<Post> list = inRepo._dataStore.Posts.OrderByDescending(p => p.GetNumberOfVotes()).Take(numPosts).ToList();

            Console.WriteLine("Posts with most votes:");
            foreach (var post in list)
                Console.WriteLine("Post by {0,-18}, votes {1}, post - {2}", post.Author.NameHtml, post.GetNumberOfVotes(), post.Title);
            Console.WriteLine("");
        }

        public static void GetPostsWithMostSumOfVotes(int numPosts, ModelRepository inRepo)
        {
            List<Post> list = inRepo._dataStore.Posts.OrderByDescending(p => p.GetSumOfVotes()).Take(numPosts).ToList();

            Console.WriteLine("Post with max sum of votes:");
            foreach (var post in list)
                Console.WriteLine("Post by {0,-18}, votes {1}, post - {2}", post.Author.NameHtml, post.GetSumOfVotes(), post.Title);
            Console.WriteLine("");
        }
        public static void GetPostsWithZeroVotes(ModelRepository inRepo)
        {
            List<Post> list = inRepo._dataStore.Posts.Where(p => p.GetNumberOfVotes() == 0).ToList();

            Console.WriteLine("Post with zero votes:");
            foreach (var post in list)
                Console.WriteLine("Post by {0,-18}, votes {1}, date - {2}, post - {3}", post.Author.NameHtml, post.GetNumberOfVotes(), post.DatePosted, post.Title);
            Console.WriteLine("");
        }
    }
}
