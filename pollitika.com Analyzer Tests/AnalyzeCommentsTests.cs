﻿using System;
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
            Assert.AreEqual(13, Analyzer.GetPostCommentsNum(htmlDocument1.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));

            HtmlDocument htmlDocument2 = htmlWeb.Load("http://pollitika.com/nitko-da-ne-dodje-do-prijatelj-drag");
            Assert.AreEqual(56, Analyzer.GetPostCommentsNum(htmlDocument2.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));

            HtmlDocument htmlDocument3 = htmlWeb.Load("http://pollitika.com/trijumf-trollova");
            Assert.AreEqual(159, Analyzer.GetPostCommentsNum(htmlDocument3.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));

            HtmlDocument htmlDocument4 = htmlWeb.Load("http://pollitika.com/kapetan-amerika-protiv-klime");
            Assert.AreEqual(51, Analyzer.GetPostCommentsNum(htmlDocument4.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));

            HtmlDocument htmlDocument5 = htmlWeb.Load("http://pollitika.com/destiliranje-viska-vrijednosti");
            Assert.AreEqual(107, Analyzer.GetPostCommentsNum(htmlDocument5.DocumentNode.Descendants().SingleOrDefault(x => x.Id == "content-main")));


        }
    }
}