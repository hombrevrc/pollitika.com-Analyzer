using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Repository.Hierarchy;

namespace TestMultithreadedQueue
{
    
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static Random rnd = new Random(5);

        public static void AnalyzePost(string inUrl)
        {
            //Console.WriteLine("Starting " + inUrl);

            ILog log = log4net.LogManager.GetLogger(typeof(Program));
            log.Info("Starting " + inUrl);

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
                ILog log2 = log4net.LogManager.GetLogger(typeof(Program));
                log2.WarnFormat("Queue length {0}", queueLength);

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
                                                        {
                                                            //Console.WriteLine("Successfully done " + url);
                                                            ILog log = log4net.LogManager.GetLogger(typeof(Program));

                                                            log.Error("Successfully done " + url);
                                                        }
                                                        Interlocked.Decrement(ref queueLength);
                                                    }
                                                );
                }
                else
                {

                    //  Console.WriteLine("Waiting");
                    Thread.Sleep(pauseInMilli);

                    ILog log = log4net.LogManager.GetLogger(typeof(Program));
                    log.Error("Waiting");

                    goto repeat;

                }
            }
        }

        static void Main(string[] args)
        {
            //log4net.Config.BasicConfigurator.Configure();
            //ILog log = log4net.LogManager.GetLogger(typeof(Program));
            //log.Debug("This is a debug message");
            //log.Warn("This is a warn message");
            //log.Error("This is a error message");
            //log.Fatal("This is a fatal message");

           // log.InfoFormat("Running as {0}", WindowsIdentity.GetCurrent().Name);
            //log.Error("This will appear in red in the console and still be written to file!");

            // generirati listu od 100 stringova
            List<string> listUrls = new List<string>();
            for(int i=0; i<13; i++)
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
