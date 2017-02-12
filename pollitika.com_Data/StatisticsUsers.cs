﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pollitika.com_Analyzer;

namespace pollitika.com_Data
{
    public class StatisticsUsers
    {
        public static void GetUserStatistics(string inUserName, ModelRepository inRepo)
        {
            User user = inRepo.GetUserByName(inUserName);

            if (user != null)
            {
                Console.WriteLine("Posts   : {0}", user.PostsByUser.Count);
                Console.WriteLine("Comments: {0}", user.CommentsByUser.Count);
                Console.WriteLine("Votes   : {0}", user.VotesByUser.Count);

                List<Post> postByDate = user.PostsByUser.OrderBy(p => p.DatePosted).ToList();
                List<Post> postByVotes = user.PostsByUser.OrderByDescending(p => p.GetNumberOfVotes()).ToList();
                List<Post> postByComments = user.PostsByUser.OrderByDescending(p => p.GetNumberOfComments()).ToList();

                Console.WriteLine("Ordered by date:");
                foreach (var post in postByDate)
                    Console.WriteLine("  Post: {0, -90}, Date: {1}, Votes: {2,3}, Num.comments: {3,3}", post.Title, post.DatePosted.ToString("dd/MM/yyy hh:mm:ss"), post.Votes.Count, post.Comments.Count);

                Console.WriteLine("Ordered by number of votes:");
                foreach (var post in postByVotes)
                    Console.WriteLine("  Post: {0, -90}, Date: {1}, Votes: {2,3}, Num.comments: {3,3}", post.Title, post.DatePosted.ToString("dd/MM/yyy hh:mm:ss"), post.Votes.Count, post.Comments.Count);

                Console.WriteLine("Ordered by number of comments:");
                foreach (var post in postByComments)
                    Console.WriteLine("  Post: {0, -90}, Date: {1}, Votes: {2,3}, Num.comments: {3,3}", post.Title, post.DatePosted.ToString("dd/MM/yyy hh:mm:ss"), post.Votes.Count, post.Comments.Count);
            }
        }
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
            List<User> list = inRepo._dataStore.Users.OrderByDescending(p => p.GetNumberOfGivenNegativeVotes()).Take(numUsers).ToList();

            Console.WriteLine("Users who gave most negative votes:");
            foreach (var user in list)
                Console.WriteLine("User {0,-18}   - votes {1}", user.NameHtml, user.GetNumberOfGivenNegativeVotes());
            Console.WriteLine("");
        }
        public static void GetUsersWhoReceivedMostNegativeVotes(int numUsers, ModelRepository inRepo)
        {
            List<User> list = inRepo._dataStore.Users.OrderByDescending(p => p.GetNumberOfReceivedNegativeVotes()).Take(numUsers).ToList();

            Console.WriteLine("Users who received most negative votes:");
            foreach (var user in list)
                Console.WriteLine("User {0,-18}   - votes {1}", user.NameHtml, user.GetNumberOfReceivedNegativeVotes());
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

            Console.WriteLine("Users with highest average number of votes per post:");
            foreach (var user in list)
                Console.WriteLine("User {0,-18}   - votes per post: {1:N2}, total posts: {2,3}", user.NameHtml, user.GetAverageVotesPerPost(), user.PostsByUser.Count);
            Console.WriteLine("");
        }

        public static void GetUsersWithBiggestAverageNumberOfVotesPerComment(int numUsers, ModelRepository inRepo)
        {
            List<User> list = inRepo._dataStore.Users.Where(p => p.CommentsByUser.Count > 10).OrderByDescending(p => p.GetAverageVotesPerComment()).Take(numUsers).ToList();

            Console.WriteLine("Users with highest average number of votes per comment:");
            foreach (var user in list)
                Console.WriteLine("User {0,-18}   - votes per comment {1:N2}, total comments {2,3}", user.NameHtml, user.GetAverageVotesPerComment(), user.CommentsByUser.Count);
            Console.WriteLine("");
        }

        public static void GetUsersWithBiggestNumberOfPostsWithOverNVotes(int numUsers, int numVotes, ModelRepository inRepo)
        {
            List<User> list = inRepo._dataStore.Users.Where(p => p.PostsByUser.Count > 10).OrderByDescending(p => p.GetNumberOfPostsWithOverNVotes(numVotes)).Take(numUsers).ToList();

            Console.WriteLine("Users with highest number of posts with over {0} votes:", numVotes);
            foreach (var user in list)
                Console.WriteLine("User {0,-18}   - votes {1}", user.NameHtml, user.GetNumberOfPostsWithOverNVotes(numVotes));
            Console.WriteLine("");
        }

    }
}
