using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using pollitika.com_AnalyzerLib;
using pollitika.com_Data;

namespace pollitika.com_ConsoleRunner
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            ModelRepository repo = new ModelRepository();
            List<string> listOfPosts = new List<string>();

            string repoName = "pollitikaNew.db";

            Logger.Info("Opening data store: " + repoName);
            repo.CreateNewDataStore(repoName);

            Logger.Info("\nFETCHING POSTS FROM FRONTPAGE:");
            for (int j = 0; j <= 600; j += 100)
                for (int i = 0; i < 1; i++)
                {
                    Logger.InfoFormat("  DOING FRONT PAGE - {0}", j + i);
                    var listPosts = FrontPageAnalyzer.GetPostLinksFromFrontPage(j + i);

                    listOfPosts.AddRange(listPosts);
                }


            Logger.Info("\nLIST OF POSTS TO ANALYZE:");
            for (int i = 0; i < listOfPosts.Count; i++)
                Logger.Info((i + 1).ToString() + ". " + listOfPosts[i]);

            SimpleMultithreadedScrapper.AnalyzeFrontPage_SimpleMultithreaded(listOfPosts, repo);

            repo.UpdateDataStore();
        }
    }
}
