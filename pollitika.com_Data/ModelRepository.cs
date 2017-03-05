using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using pollitika.com_Model;

namespace pollitika.com_Data
{
    public class ModelRepository : IModelRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static Mutex mutexAddPost = new Mutex();

        internal string     _dataStoreName = "";
        internal DataStore  _dataStore = new DataStore();

        internal string DataStoreName
        {
            get { return _dataStoreName; }
            set { _dataStoreName = value; }
        }

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

                DataStoreName = inFileName;
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

                DataStoreName = inFileName;

                // temporary fix
                //foreach (Post p in _dataStore.Posts)
                //    p.IsOnFrontPage = true;
            }
            catch (Exception e)
            {
                log.Error("Exception occured : " + e.Message);
                return false;
            }

            return true;
        }

        public bool UpdateDataStore()
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(DataStoreName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, _dataStore);
                stream.Close();
            }
            catch (Exception e)
            {
                log.Error("Exception occured : " + e.Message);
                return false;
            }
            return true;
        }

        #endregion

        public void FixVotes()
        {
            foreach (var post in _dataStore.Posts)
            {
                foreach (var vote in post.Votes)
                {
                    vote.ByUser.VotesByUser.Add(vote);
                }

                foreach (var comment in post.Comments)
                {
                    foreach (var commentVote in comment.Votes)
                    {
                        commentVote.ByUser.VotesByUser.Add(commentVote);
                    }

                }
            }
        }

        public void AddUser(User newUser)
        {
            mutexAddPost.WaitOne();

            if (newUser.NameHtml != "" && _dataStore.Users.Count(p => p.NameHtml == newUser.NameHtml) == 0)
            {
                //Console.WriteLine(newUser.Name + " ; " + newUser.NameHtml);
                _dataStore.Users.Add(newUser);
            }
            else if( newUser.NameHtml == "" && _dataStore.Users.Count(p => p.Name == newUser.Name) == 0)
            {
                log.Warn("Adding user without nick: " + newUser.Name);
                _dataStore.Users.Add(newUser);
            }

            mutexAddPost.ReleaseMutex();
        }
        public void AddPost(Post newPost)
        {
            Stopwatch timer = new Stopwatch();

            //timer.Start();
            mutexAddPost.WaitOne();
            //timer.Stop();
            //Console.WriteLine("Waited for mutex = {0}", timer.Elapsed);

            if (_dataStore.Posts.Count(p => p.Id == newPost.Id) == 0)
            {
                _dataStore.Posts.Add(newPost);

                // add this post to list of posts by user
                User user = this.GetUserByNick(newPost.Author.NameHtml);
                user.AddPostToList(newPost);

                // adding all votes to lists
                foreach (var vote in newPost.Votes)
                {
                    _dataStore.Votes.Add(vote);

                    vote.ByUser.VotesByUser.Add(vote);
                    vote.VoteForUser.ReceivedVotes.Add(vote);
                }

                // add all comments to the list of comments
                foreach (var comment in newPost.Comments)
                {
                    // for the user that made the comment, add comment to his list
                    user = this.GetUserByNick(comment.Author.NameHtml);
                    user.AddCommentToList(comment);

                    _dataStore.Comments.Add(comment);

                    foreach (var commentVote in comment.Votes)
                    {
                        _dataStore.Votes.Add(commentVote);

                        commentVote.ByUser.VotesByUser.Add(commentVote);
                        commentVote.VoteForUser.ReceivedVotes.Add(commentVote);
                    }
                }
            }

            mutexAddPost.ReleaseMutex();
        }

        public bool PostAlreadyExists(int inPostID)
        {
            if (_dataStore.Posts.Count(p => p.Id == inPostID) == 0)
                return false;
            else
                return true;
        }
        public bool PostAlreadyExists(string inPostUrl)
        {
            if (_dataStore.Posts.Count(p => p.HrefLink == inPostUrl) == 0)
                return false;
            else
                return true;
        }

        public User GetUserByName(string inName)
        {
            return _dataStore.Users.FirstOrDefault(p => p.Name == inName);
        }
        public User GetUserByNick(string inNick)
        {
            return _dataStore.Users.FirstOrDefault(p => p.NameHtml == inNick);
        }

        public void WriteListOfUsersInFile(string inFileName)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(inFileName))
            {
                foreach (User user in _dataStore.Users)
                {
                    if( user.NameHtml != "")
                        file.WriteLine(user.NameHtml);
                }
            }
        }

        public List<string> GetListOfUserNicks()
        {
            return _dataStore.Users.Where(p => p.NameHtml != "").Select(p => p.NameHtml).ToList();
        } 
    }
}
