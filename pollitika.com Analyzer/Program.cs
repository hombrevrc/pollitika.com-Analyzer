using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using log4net;
using pollitika.com_AnalyzerLib;
using pollitika.com_Data;
using pollitika.com_Model;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using ScrapySharp.Extensions;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace pollitika.com_Analyzer
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            /*string repoName = "pollitikaListTest.db";
            ModelRepository repo = new ModelRepository();

            Logger.Info("Opening data store: " + repoName);
            repo.OpenDataStore(repoName);

            List<string> listPosts = LoadListOfPostsFromFile("ListaPostova.txt");

            AnalyzePostsFromList(repo, listPosts, true);

            repo.WriteListOfUsersInFile("ListOfUsers.txt");*/



            //AnalyzeFrontPagePosts("pollitika.db");
            //CreateTestDatabase("pollitikaTest.db");
        }

        static List<string> LoadListOfPostsFromFile(string inFileName)
        {
            List<string> lines = System.IO.File.ReadAllLines(inFileName).ToList();

            return lines;
        }

        static void AnalyzePostsFromList(ModelRepository repo, List<string> inListPosts, bool isFrontPage)
        {
            Logger.Info("\nLIST OF POSTS TO ANALYZE:");
            for (int i = 0; i < inListPosts.Count; i++)
                Logger.Info((i + 1).ToString() + ". " + inListPosts[i]);

            Logger.Info("\nSTARTING ANALYSIS:");
            MultithreadedScrapper.AnalyzeListOfPosts_Multithreaded(inListPosts, repo, isFrontPage, true);

            repo.UpdateDataStore();

            PrintStatistics(repo);
        }

        static void AnalyzeFrontPagePosts(string inRepoFileName)
        {
            ModelRepository repo = new ModelRepository();
            List<string> listOfPosts = new List<string>();

            Logger.Info("Opening data store: " + inRepoFileName);
            repo.OpenDataStore(inRepoFileName);

            Logger.Info("\nFETCHING POSTS FROM FRONTPAGE:");
            for (int j = 0; j <= 500; j += 100)
                for (int i = 85; i < 100; i++)
                {
                    Logger.InfoFormat("  DOING FRONT PAGE - {0}", j + i);
                    var listPosts = FrontPageAnalyzer.GetPostLinksFromFrontPage(j + i);

                    listOfPosts.AddRange(listPosts);
                }

            Logger.Info("\nLIST OF POSTS TO ANALYZE:");
            for (int i = 0; i < listOfPosts.Count; i++)
                Logger.Info((i + 1).ToString() + ". " + listOfPosts[i]);

            Logger.Info("\nSTARTING ANALYSIS:");
            MultithreadedScrapper.AnalyzeListOfPosts_Multithreaded(listOfPosts, repo, true, true);

            repo.UpdateDataStore();

            PrintStatistics(repo);
        }

        static void CreateTestDatabase(string inFileName)
        {
            ModelRepository repo = new ModelRepository();
            List<string> listOfPosts = new List<string>();

            Logger.Info("Creating data store: " + inFileName);
            repo.CreateNewDataStore(inFileName);

            Logger.Info("\nFETCHING POSTS FROM FRONTPAGE:");
            for (int j = 0; j <= 650; j += 700)
                for (int i = 0; i < 1; i++)
                {
                    Logger.InfoFormat("  DOING FRONT PAGE - {0}", j + i);
                    var listPosts = FrontPageAnalyzer.GetPostLinksFromFrontPage(j + i);

                    listOfPosts.AddRange(listPosts);
                }

            Logger.Info("\nLIST OF POSTS TO ANALYZE:");
            for (int i = 0; i < listOfPosts.Count; i++)
                Logger.Info((i+1).ToString() + ". " + listOfPosts[i]);

            Logger.Info("\nSTARTING ANALYSIS:");
            MultithreadedScrapper.AnalyzeListOfPosts_Multithreaded(listOfPosts, repo, true, true);

            // now we will fetch posts from some users
            Logger.Info("\nANALYZING USERS POSTS:");
            listOfPosts.Clear();
            List<string> usersTofetch = new List<string>
            {
                //"mrak",
                //"zoran-ostric",
                "ppetra",
                "zvone-radikalni",
                "otpisani"
            };

            foreach (string user in usersTofetch)
            {
                var list = UserPostsAnalyzer.GetListOfUserPosts(user);

                listOfPosts.AddRange(list);
            }

            Logger.Info("\nLIST OF USERS POSTS TO ANALYZE:");
            for (int i = 0; i < listOfPosts.Count; i++)
                Logger.Info((i + 1).ToString() + ". " + listOfPosts[i]);

            Logger.Info("\nSTARTING ANALYSIS:");
            MultithreadedScrapper.AnalyzeListOfPosts_Multithreaded(listOfPosts, repo, true, true);

            repo.UpdateDataStore();

            PrintStatistics(repo);
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

        static public void FetchPosts()
        {
            // setup - repozitoriji za spremanje podataka
            ModelRepository repo = new ModelRepository();

            repo.OpenDataStore("pollitika.db");

            // prođemo kroz naslovnicu i pokupimo sve postove (i sve komentare i dodamo sve korisnike)
            for (int j = 200; j <= 600; j++)
                for (int i = 0; i < 1; i++)
                {
                    Console.WriteLine("DOING PAGE - {0}", j + i);

                    var listPosts = FrontPageAnalyzer.GetPostLinksFromFrontPage(j + i);

                    foreach (string postUrl in listPosts)
                    {
                        //Console.WriteLine("Post url {0}", postUrl);
                        Post newPost = PostAnalyzer.AnalyzePost("http://pollitika.com" + postUrl, repo, true, true);

                        if (newPost != null)
                            repo.AddPost(newPost);
                    }

                    repo.UpdateDataStore();
                }

            // zatim prolazimo kroz sve korisnike

            // i za svakog od njih proći kroz sve njegove dnevnike

            // i za svaki dnevnik provjeriti da li je već dodan, i ak onije dodati ga
        }
    }
}