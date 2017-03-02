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

            string repoName = "pollitikaNew2.db";

            Logger.Info("Opening data store: " + repoName);
            repo.CreateNewDataStore(repoName);

            Logger.Info("\nFETCHING POSTS FROM FRONTPAGE:");
            for (int j = 0; j <= 600; j += 100)
                for (int i = 1; i < 20; i++)
                {
                    Logger.InfoFormat("  DOING FRONT PAGE - {0}", j + i);
                    var listPosts = FrontPageAnalyzer.GetPostLinksFromFrontPage(j + i);

                    listOfPosts.AddRange(listPosts);
                }


            Logger.Info("\nLIST OF POSTS TO ANALYZE:");
            for (int i = 0; i < listOfPosts.Count; i++)
                Logger.Info((i + 1).ToString() + ". " + listOfPosts[i]);

            ContinuousMultiThreadedScrapper.AnalyzeListOfPosts_Multithreaded(listOfPosts, repo, true, true);

            PrintStatistics(repo);

            repo.UpdateDataStore();
        }

        public void PerformSimpleDownloadOfListOfPosts()
        {
            ModelRepository repo = new ModelRepository();
            List<string> listOfPosts = new List<string>();

            string repoName = "pollitikaNew.db";

            Logger.Info("Opening data store: " + repoName);
            repo.OpenDataStore(repoName);

            List<string> listToDo = new List<string>
            {
                "/milivoj-petkovic-naredio-da-se-dvije-brigade-hvo-a-povuku-iz-brckog-i-tuzle",
                "/drumovi-ce-pozeljet-turaka-a-turaka-nigdje-biti-nece",
                "/tajne-robnih-zaliha",
                "/bahatost-na-a-svagda-nja",
                "/moc-pozeljnog-razmi-ljanja-ili-wishful-thinking-in-cloud-cuckoo-land",
                "/policija-pobolj-ala-rad",
                "/le-web",
                "/cija-ce-volja-trijumfirati-u-radanju-jednih-nacija",
                "/sustavno-trovanje-gradana-dok-sustav-funkcionira",
                "/o-rusvaju-s-jmbg-om"
            };

            SimpleMultithreadedScrapper.AnalyzeFrontPage_SimpleMultithreaded(listToDo, repo);

            PrintStatistics(repo);

            repo.UpdateDataStore();
        }

        public static void PrintStatistics(ModelRepository repo)
        {
            StatisticsPosts.GetSummary(repo);

            StatisticsUsers.GetUsersWithMostPosts(30, repo);

            StatisticsUsers.GetUsersWithMostComments(30, repo);

            StatisticsUsers.GetUsersWhoGaveMostVotes(30, repo);

            StatisticsUsers.GetUsersWhoGaveMostNegativeVotes(30, repo);

            StatisticsUsers.GetUsersWhoReceivedMostNegativeVotes(30, repo);

            StatisticsUsers.GetUsersWithBiggestAverageNumberOfVotesPerPost(30, repo);

            StatisticsUsers.GetUsersWithBiggestAverageNumberOfVotesPerComment(30, repo);

            StatisticsUsers.GetUsersWithBiggestNumberOfPostsWithOverNVotes(30, 30, repo);

            StatisticsUsers.GetUsersWithBiggestNumberOfPostsWithOverNVotes(30, 40, repo);

            StatisticsUsers.GetUsersWithBiggestNumberOfPostsWithOverNVotes(30, 50, repo);

            StatisticsPosts.GetPostsWithMostNumberOfVotes(30, repo);

            StatisticsPosts.GetPostsWithMostSumOfVotes(30, repo);

            StatisticsPosts.GetPostsWithMostNegativeVotes(30, repo);

            StatisticsPosts.GetMostControversialPosts(30, repo);

            StatisticsPosts.GetMostCommentedPosts(30, repo);

            StatisticsUsers.GetUserStatistics("Zvone Radikalni", repo);

            //StatisticsPosts.GetPostsWithZeroVotes(repo);
        }
    }
}
