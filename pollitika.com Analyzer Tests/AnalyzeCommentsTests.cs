using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pollitika.com_Analyzer;
using pollitika.com_Data;
using pollitika.com_Model;

namespace pollitika.com_Analyzer_Tests
{
    [TestClass]
    public class AnalyzeCommentsTests
    {
        private IModelRepository _repo = new ModelRepository();

        #region Tests for ScrapePostCommentsNum
        [TestMethod]
        public void GetPostCommentsNum_Test1()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija");
            Assert.AreEqual(13, AnalyzeComments.ScrapePostCommentsNum(htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));

            HtmlDocument htmlDocument2 = htmlWeb.Load("http://pollitika.com/nitko-da-ne-dodje-do-prijatelj-drag");
            Assert.AreEqual(56, AnalyzeComments.ScrapePostCommentsNum(htmlDocument2.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));

            HtmlDocument htmlDocument3 = htmlWeb.Load("http://pollitika.com/trijumf-trollova");
            Assert.AreEqual(161, AnalyzeComments.ScrapePostCommentsNum(htmlDocument3.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));

            HtmlDocument htmlDocument4 = htmlWeb.Load("http://pollitika.com/kapetan-amerika-protiv-klime");
            Assert.AreEqual(51, AnalyzeComments.ScrapePostCommentsNum(htmlDocument4.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));

            HtmlDocument htmlDocument5 = htmlWeb.Load("http://pollitika.com/destiliranje-viska-vrijednosti");
            Assert.AreEqual(107, AnalyzeComments.ScrapePostCommentsNum(htmlDocument5.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));
        }
        #endregion


        #region Simple tests for ScrapePostComments - testing number of fetched comments
        [TestMethod]
        public void GetPostComments_Test1()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<Comment > listComments = AnalyzeComments.ScrapePostComments(mainContent, "http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija", _repo, false);

            Assert.AreEqual(13, listComments.Count);
        }

        [TestMethod]
        public void GetPostComments_Test2()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/destiliranje-viska-vrijednosti");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<Comment> listComments = AnalyzeComments.ScrapePostComments(mainContent, "http://pollitika.com/destiliranje-viska-vrijednosti", _repo, false);

            Assert.AreEqual(107, listComments.Count);
        }
        [TestMethod]
        public void GetPostComments_Test3()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/nitko-da-ne-dodje-do-prijatelj-drag");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<Comment> listComments = AnalyzeComments.ScrapePostComments(mainContent, "http://pollitika.com/nitko-da-ne-dodje-do-prijatelj-drag", _repo, false);

            Assert.AreEqual(56, listComments.Count);
        }
        [TestMethod]
        public void GetPostComments_Test4()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/trijumf-trollova");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<Comment> listComments = AnalyzeComments.ScrapePostComments(mainContent, "http://pollitika.com/trijumf-trollova", _repo, false);

            Assert.AreEqual(161, listComments.Count);
        }
        [TestMethod]
        public void GetPostComments_Test5()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/kapetan-amerika-protiv-klime");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<Comment> listComments = AnalyzeComments.ScrapePostComments(mainContent, "http://pollitika.com/kapetan-amerika-protiv-klime", _repo, false);

            Assert.AreEqual(51, listComments.Count);
        }

        [TestMethod]
        public void GetPostComments_Test6()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/nered-na-trzi-tu-dobra-stvar");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<Comment> listComments = AnalyzeComments.ScrapePostComments(mainContent, "http://pollitika.com/nered-na-trzi-tu-dobra-stvar", _repo, false);

            Assert.AreEqual(4, listComments.Count);
        }
        [TestMethod]
        public void GetPostComments_Test7()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/pollitika-kao-quotevo-siljim-drvo-da-ubijem-meduquot");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<Comment> listComments = AnalyzeComments.ScrapePostComments(mainContent, "http://pollitika.com/pollitika-kao-quotevo-siljim-drvo-da-ubijem-meduquot", _repo, false);

            Assert.AreEqual(19, listComments.Count);
        }
        [TestMethod]
        public void GetPostComments_Test8()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/socijalist-ili");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<Comment> listComments = AnalyzeComments.ScrapePostComments(mainContent, "http://pollitika.com/socijalist-ili", _repo, false);

            Assert.AreEqual(8, listComments.Count);
        }
        [TestMethod]
        public void GetPostComments_Test9()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/zamp-cekajuci-prava-pitanja");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<Comment> listComments = AnalyzeComments.ScrapePostComments(mainContent, "http://pollitika.com/zamp-cekajuci-prava-pitanja", _repo, false);

            Assert.AreEqual(519, listComments.Count);
        }
        [TestMethod]
        public void GetPostComments_Test10()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/prvi-potezi-vlade");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<Comment> listComments = AnalyzeComments.ScrapePostComments(mainContent, "http://pollitika.com/prvi-potezi-vlade", _repo, false);

            Assert.AreEqual(491, listComments.Count);
        }
        [TestMethod]
        public void GetPostComments_Test11()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/eu-skeptik");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<Comment> listComments = AnalyzeComments.ScrapePostComments(mainContent, "http://pollitika.com/eu-skeptik", _repo, false);

            Assert.AreEqual(569, listComments.Count);
        }
        [TestMethod]
        public void GetPostComments_Test12()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/socijalist-ili");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<Comment> listComments = AnalyzeComments.ScrapePostComments(mainContent, "http://pollitika.com/socijalist-ili", _repo, false);

            Assert.AreEqual(8, listComments.Count);
        }


        #endregion


        #region Testing votes in comments
        [TestMethod]
        public void GetPostComments_TestVotesOnComments()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<Comment> listComments = AnalyzeComments.ScrapePostComments(mainContent, "http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija", _repo, true);

            Assert.AreEqual(13, listComments.Count);

            Comment comm = listComments[0];
            Assert.AreEqual("Skviki", comm.Author.Name);
            Assert.AreEqual(0, comm.NumScrappedVotes);
            Assert.AreEqual(522047, comm.Id);
            Assert.AreEqual(new DateTime(2016, 11, 28, 16, 16, 0), comm.DatePosted);
            Assert.AreEqual(0, comm.NumScrappedVotes);
            Assert.AreEqual(2, comm.Votes.Count);
            Assert.AreEqual("Liberty Valance", comm.Votes[0].ByUser.Name);
            Assert.AreEqual(new DateTime(2017, 01, 28, 12, 18, 0), comm.Votes[0].DatePosted);
            Assert.AreEqual(-1, comm.Votes[0].UpOrDown);
            Assert.AreEqual("Zvone Radikalni", comm.Votes[1].ByUser.Name);
            Assert.AreEqual(new DateTime(2017, 01, 28, 12, 16, 0), comm.Votes[1].DatePosted);
            Assert.AreEqual(1, comm.Votes[1].UpOrDown);

            comm = listComments[4];
            Assert.AreEqual("magarac", comm.Author.Name);
            Assert.AreEqual(2, comm.NumScrappedVotes);
            Assert.AreEqual(521866, comm.Id);
            Assert.AreEqual(new DateTime(2016, 11, 16, 11, 19, 0), comm.DatePosted);
            Assert.AreEqual(2, comm.NumScrappedVotes);
            Assert.AreEqual(2, comm.Votes.Count);
            Assert.AreEqual("fuminanti", comm.Votes[0].ByUser.Name);
            Assert.AreEqual(new DateTime(2016, 11, 22, 10, 46, 0), comm.Votes[0].DatePosted);
            Assert.AreEqual(1, comm.Votes[0].UpOrDown);
            Assert.AreEqual("hlad", comm.Votes[1].ByUser.Name);
            Assert.AreEqual(new DateTime(2016, 11, 16, 11, 39, 0), comm.Votes[1].DatePosted);
            Assert.AreEqual(1, comm.Votes[1].UpOrDown);

            comm = listComments[6];
            Assert.AreEqual("lignja", comm.Author.Name);
            Assert.AreEqual(4, comm.NumScrappedVotes);
            Assert.AreEqual(521868, comm.Id);
            Assert.AreEqual(new DateTime(2016, 11, 16, 14, 03, 0), comm.DatePosted);
            Assert.AreEqual(4, comm.NumScrappedVotes);
            Assert.AreEqual(4, comm.Votes.Count);
            Assert.AreEqual("mario121", comm.Votes[0].ByUser.Name);
            Assert.AreEqual(new DateTime(2016, 11, 16, 22, 22, 0), comm.Votes[0].DatePosted);
            Assert.AreEqual(1, comm.Votes[0].UpOrDown);
            Assert.AreEqual("Skviki", comm.Votes[1].ByUser.Name);
            Assert.AreEqual(new DateTime(2016, 11, 16, 16, 01, 0), comm.Votes[1].DatePosted);
            Assert.AreEqual(1, comm.Votes[1].UpOrDown);
            Assert.AreEqual("indian", comm.Votes[2].ByUser.Name);
            Assert.AreEqual(new DateTime(2016, 11, 16, 15, 51, 0), comm.Votes[2].DatePosted);
            Assert.AreEqual(1, comm.Votes[2].UpOrDown);
            Assert.AreEqual("magarac", comm.Votes[3].ByUser.Name);
            Assert.AreEqual(new DateTime(2016, 11, 16, 15, 34, 0), comm.Votes[3].DatePosted);
            Assert.AreEqual(1, comm.Votes[3].UpOrDown);
        }

        #endregion

        #region Complex tests
        [TestMethod]
        public void GetPostComments_TestCompleteCommentList()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<Comment> listComments = AnalyzeComments.ScrapePostComments(mainContent, "http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija", _repo);

            Assert.AreEqual(13, listComments.Count);

            Comment comm = listComments[0];
            Assert.AreEqual("Skviki", comm.Author.Name);
            //Assert.AreEqual("skviki", comm.Author.NameHtml);
            Assert.AreEqual(0, comm.NumScrappedVotes);
            Assert.AreEqual(522047, comm.Id);
            Assert.AreEqual(new DateTime(2016, 11, 28, 16, 16, 0), comm.DatePosted);

            comm = listComments[4];
            Assert.AreEqual("magarac", comm.Author.Name);
            Assert.AreEqual(2, comm.NumScrappedVotes);
            Assert.AreEqual(521866, comm.Id);
            Assert.AreEqual(new DateTime(2016, 11, 16, 11, 19, 0), comm.DatePosted);

            comm = listComments[6];
            Assert.AreEqual("lignja", comm.Author.Name);
            Assert.AreEqual(4, comm.NumScrappedVotes);
            Assert.AreEqual(521868, comm.Id);
            Assert.AreEqual(new DateTime(2016, 11, 16, 14, 03, 0), comm.DatePosted);

            comm = listComments[10];
            Assert.AreEqual("sjenka", comm.Author.Name);
            Assert.AreEqual(2, comm.NumScrappedVotes);
            Assert.AreEqual(522048, comm.Id);
            Assert.AreEqual(new DateTime(2016, 11, 28, 17, 27, 0), comm.DatePosted);

            comm = listComments[11];
            Assert.AreEqual("ppetra", comm.Author.Name);
            Assert.AreEqual(-1, comm.NumScrappedVotes);
            Assert.AreEqual(521867, comm.Id);
            Assert.AreEqual(new DateTime(2016, 11, 16, 12, 55, 0), comm.DatePosted);
        }
        #endregion

    }
}
