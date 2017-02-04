using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using pollitika.com_Data;
using pollitika.com_Model;

namespace pollitika.com_Analyzer
{
    public class MultithreadedScrapper
    {
        static public void AnalyzeFrontPage_Multithreaded(ModelRepository repo)
        {
            List<string> listOfPosts = new List<string>();

            for (int j=0; j <=600; j+=100)
                for (int i = 20; i < 30; i++)
                {
                    Console.WriteLine("DOING PAGE - {0}", j + i);

                    var listPosts = AnalyzeFrontPage.GetPostLinksFromFrontPage(j + i);

                    listOfPosts.AddRange(listPosts);
                }

            for( int i=0; i<listOfPosts.Count; i++ )
                Console.WriteLine(listOfPosts[i]);

            for (int i = 0; i < listOfPosts.Count; i+=10)
            {
                string postUrl1 = listOfPosts[i];
                string postUrl2 = listOfPosts[i+1];
                string postUrl3 = listOfPosts[i+2];
                string postUrl4 = listOfPosts[i+3];
                string postUrl5 = listOfPosts[i + 4];
                string postUrl6 = listOfPosts[i + 5];
                string postUrl7 = listOfPosts[i + 6];
                string postUrl8 = listOfPosts[i + 7];
                string postUrl9 = listOfPosts[i + 8];
                string postUrl10 = listOfPosts[i+9];

                Parallel.Invoke(
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl1, repo),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl2, repo),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl3, repo),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl4, repo),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl5, repo),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl6, repo),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl7, repo),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl8, repo),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl9, repo),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl10, repo)
                    );

                repo.UpdateDataStore("pollitika.db");
            }
        }

        static public void AnalyzeFrontPage_Multithreaded2(ModelRepository repo)
        {
            List<string> listOfPosts = new List<string>();

            for (int j = 0; j <= 600; j += 100)
                for (int i = 30; i < 35; i++)
                {
                    Console.WriteLine("DOING PAGE - {0}", j + i);

                    var listPosts = AnalyzeFrontPage.GetPostLinksFromFrontPage(j + i);

                    listOfPosts.AddRange(listPosts);
                }

            for (int i = 0; i < listOfPosts.Count; i++)
                Console.WriteLine(listOfPosts[i]);

            int batchInd = 0;
            int batchSize = 50;
            int numBatches = listOfPosts.Count/batchSize + 1;

            while (batchInd*batchSize < listOfPosts.Count)
            {
                Console.Write("DOING BATCH {0} of {1}", batchInd, numBatches);

                int cnt = 0;
                int startInd = batchInd*batchSize;
                List<string> postsToProcessInBatch = new List<string>();

                for (int ind = startInd; ind < startInd + batchSize && ind < listOfPosts.Count; ind++)
                    postsToProcessInBatch.Add(listOfPosts[ind]);

                postsToProcessInBatch.Add(null);    // adding terminator for WebCrawl

                int k = 0;
                List<Task> listTasks = WebCrawl(() => postsToProcessInBatch[k++],
                    (url, neki_repo) => MultithreadedAnalyzePost("http://pollitika.com" + url, repo),
                    200,
                    repo);

                //Console.WriteLine("Starting wait for tasks to finish!");
                while (listTasks.Count(task => task.IsCompleted == false) > 0)
                {
                    Thread.Sleep(1000);
                }

                Console.WriteLine("Updating store");

                repo.UpdateDataStore("pollitika.db");

                batchInd++;
            }
        }
        public static List<Task> WebCrawl(Func<string> getNextUrlToCrawl,               // returns a URL or null if no more URLs 
                                          Action<string, IModelRepository> crawlUrl,    // action to crawl the URL 
                                          int pauseInMilli,                             // if all threads engaged, waits for n milliseconds
                                          IModelRepository inRepo)
        {
            const int maxQueueLength = 5;
            string currentUrl = null;
            int queueLength = 0;
            List<Task> listTasks = new List<Task>();

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
                                                         () =>  {
                                                                    crawlUrl(temp, inRepo);
                                                                }
                                                        )
                                                .ContinueWith((t) => {
                                                                        if (t.IsFaulted)
                                                                            Console.WriteLine(t.Exception.ToString());
                                                                        //else
                                                                        //    Console.WriteLine("Successfully done " + url);
                                                                        Interlocked.Decrement(ref queueLength);
                                                                     }
                                                );
                    listTasks.Add(newTask);
                }
                else
                {
                    //  Console.WriteLine("Waiting");
                    Thread.Sleep(pauseInMilli);

                    goto repeat;

                }
            }

            return listTasks;
        }

        static public void MultithreadedAnalyzePost(string postUrl, IModelRepository inRepo)
        {
            //Console.WriteLine("Starting post url {0}", postUrl);

            try
            {
                Post newPost = AnalyzePosts.AnalyzePost(postUrl, inRepo, true, true);

                if (newPost != null)
                    inRepo.AddPost(newPost);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR " + postUrl + " MSG: " + ex.Message);
            }
        }
    }
}
