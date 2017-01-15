using System;
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
            Assert.AreEqual(13, AnalyzePosts.GetPostCommentsNum(htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));

            HtmlDocument htmlDocument2 = htmlWeb.Load("http://pollitika.com/nitko-da-ne-dodje-do-prijatelj-drag");
            Assert.AreEqual(56, AnalyzePosts.GetPostCommentsNum(htmlDocument2.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));

            HtmlDocument htmlDocument3 = htmlWeb.Load("http://pollitika.com/trijumf-trollova");
            Assert.AreEqual(159, AnalyzePosts.GetPostCommentsNum(htmlDocument3.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));

            HtmlDocument htmlDocument4 = htmlWeb.Load("http://pollitika.com/kapetan-amerika-protiv-klime");
            Assert.AreEqual(51, AnalyzePosts.GetPostCommentsNum(htmlDocument4.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));

            HtmlDocument htmlDocument5 = htmlWeb.Load("http://pollitika.com/destiliranje-viska-vrijednosti");
            Assert.AreEqual(107, AnalyzePosts.GetPostCommentsNum(htmlDocument5.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));


        }
    }
}
