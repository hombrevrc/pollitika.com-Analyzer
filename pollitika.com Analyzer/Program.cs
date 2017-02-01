using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using pollitika.com_Data;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace pollitika.com_Analyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            // setup - repozitoriji za spremanje podataka
            ModelRepository repo = new ModelRepository();

            repo.OpenDataStore("pollitika.db");
            // prođemo kroz naslovnicu i pokupimo sve postove (i sve komentare i dodamo sve korisnike)

            // zatim prolazimo kroz sve korisnike

            // i za svakog od njih proći kroz sve njegove dnevnike

            // i za svaki dnevnik provjeriti da li je već dodan, i ak onije dodati ga

            for (int i = 0; i < 1; i++)
            {
                //Console.WriteLine("DOING PAGE - {0}", i);
                //AnalyzeFrontPage(i);

                string href = "http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija";
                Post newPost = AnalyzePosts.AnalyzePost(href, repo);

                repo.AddPost(newPost);
            }
            repo.UpdateDataStore("pollitika.db");
        }
    }
}
