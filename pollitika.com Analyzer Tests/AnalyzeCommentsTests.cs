using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pollitika.com_Analyzer;

namespace pollitika.com_Analyzer_Tests
{
    [TestClass]
    public class AnalyzeCommentsTests
    {
        [TestMethod]
        public void GetPostCommentsNum_Test1()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija");
            Assert.AreEqual(13, AnalyzeComments.GetPostCommentsNum(htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));

            HtmlDocument htmlDocument2 = htmlWeb.Load("http://pollitika.com/nitko-da-ne-dodje-do-prijatelj-drag");
            Assert.AreEqual(56, AnalyzeComments.GetPostCommentsNum(htmlDocument2.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));

            HtmlDocument htmlDocument3 = htmlWeb.Load("http://pollitika.com/trijumf-trollova");
            Assert.AreEqual(159, AnalyzeComments.GetPostCommentsNum(htmlDocument3.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));

            HtmlDocument htmlDocument4 = htmlWeb.Load("http://pollitika.com/kapetan-amerika-protiv-klime");
            Assert.AreEqual(51, AnalyzeComments.GetPostCommentsNum(htmlDocument4.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));

            HtmlDocument htmlDocument5 = htmlWeb.Load("http://pollitika.com/destiliranje-viska-vrijednosti");
            Assert.AreEqual(107, AnalyzeComments.GetPostCommentsNum(htmlDocument5.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));
        }

        [TestMethod]
        public void GetPostComments_Test1()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<ScrappedComment > listVotes = AnalyzeComments.GetPostComments(mainContent);

            Assert.AreEqual(13, listVotes.Count);
        }

        [TestMethod]
        public void GetPostComments_Test2()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/destiliranje-viska-vrijednosti");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<ScrappedComment> listVotes = AnalyzeComments.GetPostComments(mainContent);

            // TODO - ima tri stranice komentara
            Assert.AreEqual(107, listVotes.Count);
        }
        [TestMethod]
        public void GetPostComments_Test3()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/nitko-da-ne-dodje-do-prijatelj-drag");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<ScrappedComment> listVotes = AnalyzeComments.GetPostComments(mainContent);

            // TODO - dvije stranice s komentarima
            Assert.AreEqual(56, listVotes.Count);
        }
        [TestMethod]
        public void GetPostComments_Test4()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/trijumf-trollova");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<ScrappedComment> listVotes = AnalyzeComments.GetPostComments(mainContent);

            // TODO - 4 stranice s komentarima
            Assert.AreEqual(159, listVotes.Count);
        }
        [TestMethod]
        public void GetPostComments_Test5()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/kapetan-amerika-protiv-klime");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<ScrappedComment> listVotes = AnalyzeComments.GetPostComments(mainContent);

            // TODO dvije stranice skomentarima
            Assert.AreEqual(51, listVotes.Count);
        }

        [TestMethod]
        public void GetPostComments_Test6()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/nered-na-trzi-tu-dobra-stvar");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<ScrappedComment> listVotes = AnalyzeComments.GetPostComments(mainContent);

            // TODO dvije stranice skomentarima
            Assert.AreEqual(4, listVotes.Count);
        }
        [TestMethod]
        public void GetPostComments_Test7()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/pollitika-kao-quotevo-siljim-drvo-da-ubijem-meduquot");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<ScrappedComment> listVotes = AnalyzeComments.GetPostComments(mainContent);

            // TODO dvije stranice skomentarima
            Assert.AreEqual(19, listVotes.Count);
        }
        [TestMethod]
        public void GetPostComments_Test8()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument1 = htmlWeb.Load("http://pollitika.com/socijalist-ili");
            HtmlNode mainContent = htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main");

            List<ScrappedComment> listVotes = AnalyzeComments.GetPostComments(mainContent);

            // TODO dvije stranice skomentarima
            Assert.AreEqual(8, listVotes.Count);
        }

    }
}
