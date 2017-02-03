using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using pollitika.com_Data;
using pollitika.com_Model;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace pollitika.com_Analyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            ModelRepository repo = new ModelRepository();

            repo.OpenDataStore("pollitika.db");

            //MultithreadedScrapper.AnalyzeFrontPage_Multithreaded(repo);

            //repo.UpdateDataStore("pollitika.db");

            PrintStatistics(repo);
        }

        public static void PrintStatistics(ModelRepository repo)
        {
            repo.GetUsersWithMostPosts(30);

            repo.GetUsersWithMostComments(30);

            repo.GetPostsWithMostNumberOfVotes(50);

            repo.GetPostsWithMostSumOfVotes(50);

            repo.GetPostsWithZeroVotes();
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
