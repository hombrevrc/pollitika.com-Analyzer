﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Repository.Hierarchy;
using pollitika.com_AnalyzerLib;
using pollitika.com_Data;
using ScrapySharp.Network;

namespace pollitika.com_ConsoleRunner
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            ModelRepository repo = new ModelRepository();
            //List<string> listOfPosts = new List<string>();

            string repoName = "../../../Data/pollitikaNew.db";
            // string repoName = "../../../Data/pollitikaNew.First_020.db";

            // string repoName = "../../../Data/pollitikaNew.First_001.db";

            Logger.Info("Opening data store: " + repoName);
            repo.OpenDataStore(repoName);

            //AnalyzeUsersPosts("../../../Data/UsersLists", repo);

            PrintStatistics(repo);

            //CreateListOfPostsForEachUser("../../../Data/CompleteListOfUsers.txt");

            //List<string> listUsers = new List<string>() {"zvone-radikalni", "dr-lesar", "marival"};
            //CreateListOfPostsForListOfUsers(listUsers);

            //GetListOfFrontPagePostsToFile("../../../Data/FrontPage_ListOfPosts.txt");
        }

        public static void AnalyzeUsersPosts(string inDirWithLists, ModelRepository repo)
        {
            // učitavaj jedan po jedan txt fajl iz direktorija
            DirectoryInfo d = new DirectoryInfo(inDirWithLists);
            FileInfo[] Files = d.GetFiles("*.txt");

            List<ScrapingBrowser> listLoggedBrowsers = new List<ScrapingBrowser>();

            Logger.Info("Logging in browsers");
            const int MaxConcurrentBrowsers = 8;
            for (int i = 0; i < MaxConcurrentBrowsers; i++)
                listLoggedBrowsers.Add(Utility.GetLoggedBrowser());

            int c = 0;
            int notSavedPosts = 0;
            foreach (FileInfo file in Files)
            {
                Logger.Info("WORKING FILE: " + file.FullName);

                // učitaj listu svih postova od korisnika
                List<string> listPosts = LoadListOfPostsFromFile(file.FullName);
                List<string> postsToAdd = new List<string>();

                // izbacit ćemo postove koji su već u bazi
                foreach(string s in listPosts)
                    if (repo.PostAlreadyExists(s) == false)
                        postsToAdd.Add(s);

                notSavedPosts += postsToAdd.Count;

                // analiziraj sve postove i dodaj ih u bazu
                ContinuousMultiThreadedScrapper.AnalyzeListOfPosts_Multithreaded_OneBatch(postsToAdd, repo, listLoggedBrowsers, false, true);

                // prebaci fajl s postovima od korisnika u Done
                File.Move(file.FullName, "../../../Data/UsersLists/Done/" + file.Name);

                if (notSavedPosts > 100)
                {
                    Logger.Info("Updating store");

                    repo.UpdateDataStore();

                    notSavedPosts = 0;
                }
            }
        }
        public static List<string> LoadListOfPostsFromFile(string inFileName)
        {
            List<string> lines = System.IO.File.ReadAllLines(inFileName).ToList();

            return lines;
        }

        public static void CreateListOfPostsForEachUser(string inFileNameWithUsersList)
        {
            using (System.IO.StreamReader file = new System.IO.StreamReader(inFileNameWithUsersList))
            {
                string userName;
                int c = 0;
                while((userName = file.ReadLine()) != null)
                {
                    try
                    {
                        List<string> userPostList = UserPostsAnalyzer.GetListOfUserPosts(userName);

                        if (userPostList.Count > 0)
                        {
                            string userFileName = "../../../Data/UsersLists/" + userName + ".txt";
                            using (System.IO.StreamWriter fileUser = new System.IO.StreamWriter(userFileName))
                            {
                                foreach (string post in userPostList)
                                    fileUser.WriteLine(post);
                            }

                            foreach (var s in userPostList)
                                Logger.Info("   " + s);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Exception thrown. Error message: " + ex.Message);
                    }

                }
            }
        }

        public static void CreateListOfUsersForRepo(ModelRepository repo, string inFileName)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(inFileName))
            {
                var list = repo.GetListOfUserNicks();

                list.Sort();

                foreach (var user in list)
                    file.WriteLine(user);
            }
        }

        public static void CreateListOfPostsForListOfUsers(List<string> listUserNames)
        {
            foreach (var userName in listUserNames)
            {
                //Logger.InfoFormat("Doing user: {0}", userName);

                List<string> userList = UserPostsAnalyzer.GetListOfUserPosts(userName);

                string userFileName = "../../../Data/UsersLists/" + userName;
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(userFileName))
                {
                    foreach (string post in userList)
                        file.WriteLine(post);
                }
            }
        }

        public static void GetListOfFrontPagePostsToFile(string inFileName)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(inFileName))
            {
                List<string> listOfPosts = new List<string>();

                for (int i = 0; i < 681; i++)
                {
                    Logger.InfoFormat("  DOING FRONT PAGE - {0}", i);

                    var listPosts = FrontPageAnalyzer.GetPostLinksFromFrontPage(i);

                    for (int j = 0; j < listPosts.Count; j++)
                    {
                        Logger.Info("  ." + listPosts[j]);
                        file.WriteLine(listPosts[j]);
                    }
                    listOfPosts.AddRange(listPosts);
                }
            }
        }
        public static void StandardDownloadFromFrontpage()
        {
            ModelRepository repo = new ModelRepository();
            List<string> listOfPosts = new List<string>();

            string repoName = "pollitikaNew.db";
            Logger.Info("Opening data store: " + repoName);
            repo.OpenDataStore(repoName);

            Logger.Info("\nFETCHING POSTS FROM FRONTPAGE:");
            for (int j = 0; j <= 500; j += 100)
                for (int i = 80; i < 100; i++)           // došli smo do 60
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

            StatisticsPosts.GetPostsWithMostNumberOfVotes(30, repo);

            StatisticsPosts.GetPostsWithMostSumOfVotes(30, repo);

            StatisticsPosts.GetPostsWithMostNegativeVotes(30, repo);

            StatisticsPosts.GetMostControversialPosts(30, repo);

            StatisticsPosts.GetMostCommentedPosts(30, repo);

            StatisticsUsers.GetUsersWithMostPosts(30, repo);

            StatisticsUsers.GetUsersWithMostComments(30, repo);

            StatisticsUsers.GetUsersWhoGaveMostVotes(30, repo);

            StatisticsUsers.GetUsersWhoGaveMostNegativeVotes(30, repo);

            StatisticsUsers.GetUsersWhoReceivedMostNegativeVotes(30, repo);

            StatisticsUsers.GetUsersWithBiggestAverageNumberOfVotesPerPost(30, repo);

            StatisticsUsers.GetUsersWithBiggestAverageNumberOfVotesPerComment(30, repo);

            StatisticsUsers.GetUsersWithBiggestAverageNumberOfCommentsPerPost(30, repo);

            StatisticsUsers.GetUsersWithBiggestNumberOfPostsWithOverNVotes(30, 30, repo);

            StatisticsUsers.GetUsersWithBiggestNumberOfPostsWithOverNVotes(30, 40, repo);

            StatisticsUsers.GetUsersWithBiggestNumberOfPostsWithOverNVotes(30, 50, repo);

            //StatisticsUsers.GetNumberOfPostInMonthAllTime(repo);

            StatisticsUsers.GetNumberOfPostMonthlyWithFrontpageStat(repo);

            StatisticsUsers.GetNumberOfPostWithOverNVotesMonthlyAllTime(30, repo);
            StatisticsUsers.GetNumberOfPostWithOverNVotesMonthlyAllTime(40, repo);
            StatisticsUsers.GetNumberOfPostWithOverNVotesMonthlyAllTime(50, repo);

            StatisticsUsers.GetNumberOfCommentsMonthlyAllTime(repo);

            StatisticsUsers.GetNumberOfActiveUsersOverTime(repo);

            // StatisticsUsers.GetUserStatistics("Zvone Radikalni", repo);

            //StatisticsPosts.GetPostsWithZeroVotes(repo);
        }
    }
}
