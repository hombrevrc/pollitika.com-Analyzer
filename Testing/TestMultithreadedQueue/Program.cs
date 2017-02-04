using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestMultithreadedQueue
{
    
    class Program
    {
        static Random rnd = new Random(5);

        public static void AnalyzePost(string inUrl)
        {
            Console.WriteLine("Starting " + inUrl);
            Thread.Sleep(1500 + rnd.Next(5000));
        }

        public static void WebCrawl(Func<string> getNextUrlToCrawl, // returns a URL or null if no more URLs 
                                    Action<string> crawlUrl, // action to crawl the URL 
                                    int pauseInMilli // if all threads engaged, waits for n milliseconds
        )
        {
            const int maxQueueLength = 5;
            string currentUrl = null;
            int queueLength = 0;

            while ((currentUrl = getNextUrlToCrawl()) != null)
            {
                repeat:
                //Console.WriteLine("Queue length {0}", queueLength);

                string temp = currentUrl;
                if (queueLength < maxQueueLength)
                {
                    var url = currentUrl;               // needed for closure capture
                    Interlocked.Increment(ref queueLength);
                    Task.Factory.StartNew(() =>
                                                {
                                                    crawlUrl(temp);
                                                } )
                                .ContinueWith((t) =>{
                                                        if (t.IsFaulted)
                                                            Console.WriteLine(t.Exception.ToString());
                                                        else
                                                            Console.WriteLine("Successfully done " + url);
                                                        Interlocked.Decrement(ref queueLength);
                                                    }
                                                );
                }
                else
                {
                    //  Console.WriteLine("Waiting");
                    Thread.Sleep(pauseInMilli);

                    goto repeat;

                }
            }
        }

        static void Main(string[] args)
        {
            // generirati listu od 100 stringova
            List<string> listUrls = new List<string>();
            for(int i=0; i<23; i++)
                listUrls.Add("Post " + i.ToString());
            listUrls.Add(null);

            int workThr, compl;
            ThreadPool.GetMaxThreads(out workThr, out compl);

            Random r = new Random();
            int j = 0;
            WebCrawl(() => listUrls[j++],
                (url) => AnalyzePost(url),
                500);
        }
    }
}
