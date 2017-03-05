using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using pollitika.com_AnalyzerLib;
using pollitika.com_Data;
using pollitika.com_Model;
using ScrapySharp.Network;

namespace pollitika.com_ConsoleRunner
{
    public class ContinuousMultiThreadedScrapper
    {
        static public void AnalyzeListOfPosts_Multithreaded_OneBatch(List<string> listOfPosts, ModelRepository repo, List<ScrapingBrowser> listLoggedBrowsers, bool isFrontPage, bool fetchCommentVotes)
        {
            Stopwatch timer = new Stopwatch();
            ILog log = log4net.LogManager.GetLogger(typeof(Program));

            List<string> postsToProcessInBatch = new List<string>();

            foreach(string s in listOfPosts)
            {
                string postUrl = "http://pollitika.com" + s;
                if (repo.PostAlreadyExists(postUrl) == false)
                    postsToProcessInBatch.Add(s);
                else
                    log.WarnFormat("Post with url {0} ALREADY EXISTS IN DATABASE", s);
            }

            postsToProcessInBatch.Add(null);    // adding terminator for CrawlListOfPages

            int k = 0;
            List<Task> listTasks = CrawlListOfPages(() => postsToProcessInBatch[k++],
                                            (url, neki_repo, browser) => SimpleMultithreadedScrapper.MultithreadedAnalyzePost("http://pollitika.com" + url, repo, isFrontPage, fetchCommentVotes, browser),
                                            1000,
                                            repo,
                                            listLoggedBrowsers);

            // when we get to the end of call, some threads are still running
            log.Debug("Starting wait for tasks to finish!");
            while (listTasks.Count(task => task.IsCompleted == false) > 0)
            {
                Thread.Sleep(1000);
            }

            log.InfoFormat("BATCH DONE {0}, BATCH DONE IN TIME {1}", DateTime.Now, timer.Elapsed);
            timer.Restart();

        }
        static public void AnalyzeListOfPosts_Multithreaded(List<string> listOfPosts, ModelRepository repo, bool isFrontPage, bool fetchCommentVotes)
        {
            Stopwatch timer = new Stopwatch();
            ILog log = log4net.LogManager.GetLogger(typeof(Program));

            List<ScrapingBrowser> listLoggedBrowsers = new List<ScrapingBrowser>();

            log.Info("Logging in browsers");
            const int MaxConcurrentBrowsers = 8;
            for(int i=0; i<MaxConcurrentBrowsers; i++)
                listLoggedBrowsers.Add(Utility.GetLoggedBrowser());

            int batchInd = 0;
            int batchSize = 50;
            int numBatches = listOfPosts.Count / batchSize + 1;

            timer.Start();

            while (batchInd * batchSize < listOfPosts.Count)
            {

                log.InfoFormat("DOING BATCH {0} of {1}, date: {2}", batchInd + 1, numBatches, DateTime.Now);

                int startInd = batchInd * batchSize;
                List<string> postsToProcessInBatch = new List<string>();

                for (int ind = startInd; ind < startInd + batchSize && ind < listOfPosts.Count; ind++)
                {
                    string postUrl = "http://pollitika.com" + listOfPosts[ind];
                    if (repo.PostAlreadyExists(postUrl) == false)
                        postsToProcessInBatch.Add(listOfPosts[ind]);
                    else
                        log.WarnFormat("Post with url {0} ALREADY EXISTS IN DATABASE", listOfPosts[ind]);
                }

                postsToProcessInBatch.Add(null);    // adding terminator for CrawlListOfPages

                int k = 0;
                List<Task> listTasks = CrawlListOfPages(() => postsToProcessInBatch[k++],
                                                (url, neki_repo, browser) => SimpleMultithreadedScrapper.MultithreadedAnalyzePost("http://pollitika.com" + url, repo, isFrontPage, fetchCommentVotes, browser),
                                                1000,
                                                repo,
                                                listLoggedBrowsers);

                // when we get to the end of call, some threads are still running
                log.Debug("Starting wait for tasks to finish!");
                while (listTasks.Count(task => task.IsCompleted == false) > 0)
                {
                    Thread.Sleep(1000);
                }

                log.Info("Updating store");

                repo.UpdateDataStore();

                log.InfoFormat("BATCH DONE {0}, BATCH DONE IN TIME {1}", DateTime.Now, timer.Elapsed);
                timer.Restart();

                batchInd++;
            }
        }
        public static List<Task> CrawlListOfPages(Func<string> getNextUrlToCrawl,               // returns a URL or null if no more URLs 
                                          Action<string, IModelRepository, ScrapingBrowser> crawlUrl,    // action to crawl the URL 
                                          int pauseInMilli,                             // if all threads engaged, waits for n milliseconds
                                          IModelRepository inRepo,
                                          List<ScrapingBrowser> listLoggedBrowsers )
        {
            Mutex mutexAccessListUsedBrowsers = new Mutex();

            int maxQueueLength = listLoggedBrowsers.Count;
            string currentUrl = null;
            int queueLength = 0;

            List<Task> listTasks = new List<Task>();
            List<bool> listIsBrowserUsed = listLoggedBrowsers.Select(a => false).ToList();

            ILog log = log4net.LogManager.GetLogger(typeof(Program));

            while ((currentUrl = getNextUrlToCrawl()) != null)
            {
                repeat:
                //Console.WriteLine("Queue length {0}", queueLength);

                string temp = currentUrl;
                if (queueLength < maxQueueLength)
                {
                    // acquire mutex
                    mutexAccessListUsedBrowsers.WaitOne();

                    int unusedBrowserIndex = -1;
                    // find first unused browser
                    for(int i=0; i<listIsBrowserUsed.Count; i++)
                        if (listIsBrowserUsed[i] == false)
                        {
                            unusedBrowserIndex = i;
                            listIsBrowserUsed[i] = true;
                            mutexAccessListUsedBrowsers.ReleaseMutex();
                            log.DebugFormat("Using browser index - {0}", unusedBrowserIndex);
                            break;
                        }

                    if (unusedBrowserIndex == -1)
                        log.Error("NO AVAILABLE BROWSERS!!!");
                    else
                    {
                        ScrapingBrowser freeBrowser = listLoggedBrowsers[unusedBrowserIndex];

                        var url = currentUrl; // needed for closure capture
                        Interlocked.Increment(ref queueLength);
                        Task newTask = Task.Factory.StartNew(
                                                    () => {
                                                              crawlUrl(temp, inRepo, freeBrowser);
                                                    }
                                                    )
                                                    .ContinueWith(
                                                        (t) =>
                                                        {
                                                            if (t.IsFaulted)
                                                                log.Error(t.Exception.ToString());
                                                            else
                                                                log.Debug("Successfully analyzed " + url);
                                                            Interlocked.Decrement(ref queueLength);

                                                            // set Browser as unused
                                                            // acquire mutex
                                                            mutexAccessListUsedBrowsers.WaitOne();

                                                            int ind = -1;
                                                            // find first unused browser
                                                            for (int i = 0; i < listLoggedBrowsers.Count; i++)
                                                                if (listLoggedBrowsers[i] == freeBrowser)
                                                                {
                                                                    listIsBrowserUsed[i] = false;
                                                                    mutexAccessListUsedBrowsers.ReleaseMutex();
                                                                    log.DebugFormat("Releasing browser index - {0}", i);

                                                                    break;
                                                                }
                                                        }
                                                    );
                        listTasks.Add(newTask);
                    }
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
    }
}
