using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using pollitika.com_Data;
using pollitika.com_Model;

namespace pollitika.com_Analyzer
{
    public class MultithreadedScrapper
    {
        static public void AnalyzeFrontPage_Multithreaded(ModelRepository repo)
        {
            List<string> listOfPosts = new List<string>();

            for (int j=600; j <=600; j+=100)
                for (int i = 19; i < 20; i++)
                {
                    Console.WriteLine("DOING PAGE - {0}", j + i);

                    var listPosts = AnalyzeFrontPage.GetPostLinksFromFrontPage(j + i);

                    listOfPosts.AddRange(listPosts);
                }

            for( int i=0; i<listOfPosts.Count; i++ )
                Console.WriteLine(listOfPosts[i]);

            for (int i = 0; i < listOfPosts.Count; i+=5)
            {
                string postUrl1 = listOfPosts[i];
                string postUrl2 = listOfPosts[i+1];
                string postUrl3 = listOfPosts[i+2];
                string postUrl4 = listOfPosts[i+3];
                string postUrl5 = listOfPosts[i+4];

                Parallel.Invoke(
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl1, repo),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl2, repo),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl3, repo),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl4, repo),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl5, repo)
                    );

                repo.UpdateDataStore("pollitika.db");
            }
        }

        static public void MultithreadedAnalyzePost(string postUrl, IModelRepository inRepo)
        {
            //Console.WriteLine("Post url {0}", postUrl);

            try
            {
                Post newPost = AnalyzePosts.AnalyzePost(postUrl, inRepo, true, true);

                if (newPost != null)
                    inRepo.AddPost(newPost);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR " + ex.Message);
            }
        }
    }
}
