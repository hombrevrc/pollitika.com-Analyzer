﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using log4net;
using pollitika.com_Data;
using pollitika.com_Model;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace pollitika.com_Analyzer
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            ModelRepository repo = new ModelRepository();

            Logger.Info("Opening data store");

            repo.OpenDataStore("pollitika.db");

            Logger.Info("Data store opened");

            //MultithreadedScrapper.AnalyzeFrontPage_Multithreaded2(repo);

            //Console.WriteLine("FIXING");
            //repo.FixVotes();

            //repo.UpdateDataStore("pollitika.db");

            PrintStatistics(repo);
        }

        public static void PrintStatistics(ModelRepository repo)
        {
            StatisticsPosts.GetSummary(repo);

            StatisticsUsers.GetUsersWithMostPosts(30, repo);

            StatisticsUsers.GetUsersWithMostComments(30, repo);

            StatisticsUsers.GetUsersWhoGaveMostVotes(50, repo);

            StatisticsUsers.GetUsersWhoGaveMostNegativeVotes(50, repo);

            StatisticsUsers.GetUsersWithBiggestAverageNumberOfVotesPerPost(50, repo);

            StatisticsUsers.GetUsersWithBiggestAverageNumberOfVotesPerComment(50, repo);

            StatisticsPosts.GetPostsWithMostNumberOfVotes(50, repo);

            StatisticsPosts.GetPostsWithMostSumOfVotes(50, repo);

            StatisticsPosts.GetPostsWithMostNegativeVotes(50, repo);

            StatisticsPosts.GetMostControversialPosts(50, repo);

            StatisticsPosts.GetMostCommentedPosts(50, repo);

            StatisticsUsers.GetUserStatistics("Zvone Radikalni", repo);

            //StatisticsPosts.GetPostsWithZeroVotes(repo);
        }

        public static void FixOnFrontPage()
        {
            ModelRepository repo = new ModelRepository();

            repo.OpenDataStore("pollitika.db");

            //foreach(var post in repo.)
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

                    var listPosts = AnalyzeFrontPage.GetPostLinksFromFrontPage(j + i);

                    foreach (string postUrl in listPosts)
                    {
                        //Console.WriteLine("Post url {0}", postUrl);
                        Post newPost = AnalyzePosts.AnalyzePost("http://pollitika.com" + postUrl, repo, true, true);

                        if (newPost != null)
                            repo.AddPost(newPost);
                    }

                    repo.UpdateDataStore("pollitika.db");
                }

            // zatim prolazimo kroz sve korisnike

            // i za svakog od njih proći kroz sve njegove dnevnike

            // i za svaki dnevnik provjeriti da li je već dodan, i ak onije dodati ga
        }
    }
}
