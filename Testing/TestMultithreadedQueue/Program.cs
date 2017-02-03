using System;
using System.Collections.Generic;
using System.Linq;
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
            Thread.Sleep(rnd.Next(500));
        }

        static void Main(string[] args)
        {
            // generirati listu od 100 stringova
            List<string> listUrls = new List<string>();
            for(int i=0; i<100; i++)
                listUrls.Add("Post " + i.ToString());

            int workThr, compl;
            ThreadPool.GetMaxThreads(out workThr, out compl);
        }
    }
}
