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

            //Console.WriteLine("DOING PAGE - {0}", i);
            var listPosts = AnalyzeFrontPage.GetPostLinksFromFrontPage(0);

            foreach (string postUrl in listPosts)
            {
                //Console.WriteLine("Post url {0}", postUrl);
                Post newPost = AnalyzePosts.AnalyzePost("http://pollitika.com" + postUrl, repo, true);

                if( newPost != null )
                    repo.AddPost(newPost);
            }

            repo.UpdateDataStore("pollitika.db");
        }
    }
}
