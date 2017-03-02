using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

using pollitika.com_AnalyzerLib;
using pollitika.com_Data;
using pollitika.com_Model;
using ScrapySharp.Network;

namespace pollitika.com_ConsoleRunner
{
    class SimpleMultithreadedScrapper
    {
        static public void AnalyzeFrontPage_SimpleMultithreaded(List<string> listOfPosts, ModelRepository repo)
        {
            ScrapingBrowser sc1 = Utility.GetLoggedBrowser();
            ScrapingBrowser sc2 = Utility.GetLoggedBrowser();
            ScrapingBrowser sc3 = Utility.GetLoggedBrowser();
            ScrapingBrowser sc4 = Utility.GetLoggedBrowser();
            ScrapingBrowser sc5 = Utility.GetLoggedBrowser();
            ScrapingBrowser sc6 = Utility.GetLoggedBrowser();
            ScrapingBrowser sc7 = Utility.GetLoggedBrowser();
            ScrapingBrowser sc8 = Utility.GetLoggedBrowser();
            ScrapingBrowser sc9 = Utility.GetLoggedBrowser();
            ScrapingBrowser sc10 = Utility.GetLoggedBrowser();

            for (int i = 0; i < listOfPosts.Count; i += 10)
            {
                string postUrl1 = listOfPosts[i];
                string postUrl2 = listOfPosts[i + 1];
                string postUrl3 = listOfPosts[i + 2];
                string postUrl4 = listOfPosts[i + 3];
                string postUrl5 = listOfPosts[i + 4];
                string postUrl6 = listOfPosts[i + 5];
                string postUrl7 = listOfPosts[i + 6];
                string postUrl8 = listOfPosts[i + 7];
                string postUrl9 = listOfPosts[i + 8];
                string postUrl10 = listOfPosts[i + 9];

                Parallel.Invoke(
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl1, repo, true, true, sc1),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl2, repo, true, true, sc2),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl3, repo, true, true, sc3),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl4, repo, true, true, sc4),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl5, repo, true, true, sc5),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl6, repo, true, true, sc6),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl7, repo, true, true, sc7),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl8, repo, true, true, sc8),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl9, repo, true, true, sc9),
                        () => MultithreadedAnalyzePost("http://pollitika.com" + postUrl10, repo, true, true, sc10)
                    );

                repo.UpdateDataStore();
            }
        }

        static public void MultithreadedAnalyzePost(string postUrl, IModelRepository inRepo, bool isFrontPage, bool fetchCommentVotes, ScrapingBrowser inBrowser)
        {
            ILog log = log4net.LogManager.GetLogger(typeof(Program));

            log.DebugFormat("Starting post url {0}", postUrl);

            try
            {
                Post newPost = PostAnalyzer.AnalyzePost(postUrl, inRepo, isFrontPage, fetchCommentVotes, inBrowser);

                if (newPost != null)
                    inRepo.AddPost(newPost);
            }
            catch (Exception ex)
            {
                log.Error("ERROR " + postUrl + " MSG: " + ex.Message);
            }
        }
    }
}
