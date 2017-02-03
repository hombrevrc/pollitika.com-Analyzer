﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pollitika.com_Analyzer;
using pollitika.com_Model;

namespace pollitika.com_Data
{
    public class ModelRepository : IModelRepository
    {
        private DataStore _dataStore = new DataStore();

        #region Data Store operations

        // ove dvije operacije imaju kao postcondition inicijaliziran JumanjiData objekt u skladu sa sadržajem fajla
        public bool CreateNewDataStore(string inFileName)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(inFileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, _dataStore);
                stream.Close();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool   OpenDataStore(string inFileName)
        {
            DataStore LoadedData;

            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(inFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                LoadedData = (DataStore)formatter.Deserialize(stream);
                stream.Close();

                _dataStore.Clear();
                _dataStore = LoadedData;
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception occured : " + e.Message);
                return false;
            }

            return true;
        }

        public bool UpdateDataStore(string inFileName)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(inFileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, _dataStore);
                stream.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception occured : " + e.Message);
                return false;
            }
            return true;
        }

        #endregion
        public void AddPost(Post newPost)
        {
            if (_dataStore.Posts.Count(p => p.Id == newPost.Id) == 0)
            {
                _dataStore.Posts.Add(newPost);

                // add this post to list of posts by user
                User user = this.GetUserByNick(newPost.Author.NameHtml);
                user.AddPostToList(newPost);

                // adding all votes to lists
                foreach (var vote in newPost.Votes)
                    _dataStore.Votes.Add(vote);

                // add all comments to the list of comments
                foreach (var comment in newPost.Comments)
                {
                    // for the user that made the comment, add comment to his list
                    user = this.GetUserByNick(comment.Author.NameHtml);
                    user.AddCommentToList(comment);

                    _dataStore.Comments.Add(comment);

                    foreach (var commentVote in comment.Votes)
                        _dataStore.Votes.Add(commentVote);
                }
            }
        }

        public bool PostAlreadyExists(int inPostID)
        {
            if (_dataStore.Posts.Count(p => p.Id == inPostID) == 0)
                return false;
            else
                return true;
        }


        public void AddUser(User newUser)
        {
            if( _dataStore.Users.Count(p => p.NameHtml == newUser.NameHtml) == 0)
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

        public void GetUsersWithMostPosts(int numUsers)
        {
            List<User> list = _dataStore.Users.OrderByDescending(p => p.PostsByUser.Count).Take(numUsers).ToList();

            Console.WriteLine("Users with most posts:");
            foreach (var user in list)
                Console.WriteLine("User {0}   - posts {1}", user.NameHtml, user.PostsByUser.Count);
            Console.WriteLine("");
        }
        public void GetUsersWithMostComments(int numUsers)
        {
            List<User> list = _dataStore.Users.OrderByDescending(p => p.CommentsByUser.Count).Take(numUsers).ToList();

            Console.WriteLine("Users with most comments:");
            foreach (var user in list)
                Console.WriteLine("User {0}   - comments {1}", user.NameHtml, user.CommentsByUser.Count);
            Console.WriteLine("");
        }

        public void GetPostsWithMostNumberOfVotes(int numPosts)
        {
            List<Post> list = _dataStore.Posts.OrderByDescending(p => p.GetNumberOfVotes()).Take(numPosts).ToList();

            Console.WriteLine("Posts with most votes:");
            foreach (var post in list)
                Console.WriteLine("Post {0}   - votes {1}", post.Title, post.GetNumberOfVotes());
            Console.WriteLine("");
        }

        public void GetPostsWithMostSumOfVotes(int numPosts)
        {
            List<Post> list = _dataStore.Posts.OrderByDescending(p => p.GetSumOfVotes()).Take(numPosts).ToList();

            Console.WriteLine("Post with max sum of votes:");
            foreach (var post in list)
                Console.WriteLine("Post {0}   - sum of votes {1}", post.Title, post.GetSumOfVotes());
            Console.WriteLine("");
        }

    }
}
