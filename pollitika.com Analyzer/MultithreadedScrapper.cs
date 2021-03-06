﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using pollitika.com_AnalyzerLib;
using pollitika.com_Data;
using pollitika.com_Model;

namespace pollitika.com_Analyzer
{
    public class MultithreadedScrapper
    {
        static public void AnalyzeListOfPosts_Multithreaded(List<string> listOfPosts, ModelRepository repo, bool isFrontPage, bool fetchCommentVotes)
        {
            ILog log = log4net.LogManager.GetLogger(typeof(Program));

            int batchInd = 0;
            int batchSize = 50;
            int numBatches = listOfPosts.Count/batchSize + 1;

            while (batchInd*batchSize < listOfPosts.Count)
            {
                log.InfoFormat("DOING BATCH {0} of {1}", batchInd+1, numBatches);

                int startInd = batchInd*batchSize;
                List<string> postsToProcessInBatch = new List<string>();

                for (int ind = startInd; ind < startInd + batchSize && ind < listOfPosts.Count; ind++)
                {
                    string postUrl = "http://pollitika.com" + listOfPosts[ind];
                    if ( repo.PostAlreadyExists(postUrl) == false )
                        postsToProcessInBatch.Add(listOfPosts[ind]);
                    else
                        log.WarnFormat("Post with url {0} ALREADY EXISTS IN DATABASE", listOfPosts[ind]);
                }

                postsToProcessInBatch.Add(null);    // adding terminator for WebCrawl

                int k = 0;
                List<Task> listTasks = WebCrawl(() => postsToProcessInBatch[k++],
                                                (url, neki_repo) => MultithreadedAnalyzePost("http://pollitika.com" + url, repo, isFrontPage, fetchCommentVotes),
                                                1000,
                                                repo);

                log.Debug("Starting wait for tasks to finish!");
                while (listTasks.Count(task => task.IsCompleted == false) > 0)
                {
                    Thread.Sleep(1000);
                }

                log.Info("Updating store");

                repo.UpdateDataStore();

                batchInd++;
            }
        }
        public static List<Task> WebCrawl(Func<string> getNextUrlToCrawl,               // returns a URL or null if no more URLs 
                                          Action<string, IModelRepository> crawlUrl,    // action to crawl the URL 
                                          int pauseInMilli,                             // if all threads engaged, waits for n milliseconds
                                          IModelRepository inRepo)
        {
            const int maxQueueLength = 8;
            string currentUrl = null;
            int queueLength = 0;
            List<Task> listTasks = new List<Task>();

            ILog log = log4net.LogManager.GetLogger(typeof(Program));

            while ((currentUrl = getNextUrlToCrawl()) != null)
            {
                repeat:
                //Console.WriteLine("Queue length {0}", queueLength);

                string temp = currentUrl;
                if (queueLength < maxQueueLength)
                {
                    var url = currentUrl;               // needed for closure capture
                    Interlocked.Increment(ref queueLength);
                    Task newTask = Task.Factory.StartNew(
                                                    () => {
                                                            crawlUrl(temp, inRepo);
                                                          }
                                                )
                                                .ContinueWith(
                                                    (t) => {
                                                            if (t.IsFaulted)
                                                                log.Error(t.Exception.ToString());
                                                            else
                                                                log.Debug("Successfully analyzed " + url);
                                                            Interlocked.Decrement(ref queueLength);
                                                           }
                                                );
                    listTasks.Add(newTask);
                }
                else
                {
                    log.Debug("Waiting for free thread");
                    Thread.Sleep(pauseInMilli);

                    goto repeat;
                }
            }

            return listTasks;
        }

        static public void MultithreadedAnalyzePost(string postUrl, IModelRepository inRepo, bool isFrontPage, bool fetchCommentVotes)
        {
            ILog log = log4net.LogManager.GetLogger(typeof(Program));

            log.DebugFormat("Starting post url {0}", postUrl);

            try
            {
                Post newPost = PostAnalyzer.AnalyzePost(postUrl, inRepo, isFrontPage, fetchCommentVotes);

                if (newPost != null)
                    inRepo.AddPost(newPost);
            }
            catch (Exception ex)
            {
                log.Error("ERROR " + postUrl + " MSG: " + ex.Message);
            }
        }

 

    }
}
