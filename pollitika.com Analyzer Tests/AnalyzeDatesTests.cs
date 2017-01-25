using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pollitika.com_Analyzer;

namespace pollitika.com_Analyzer_Tests
{
    [TestClass]
    public class AnalyzeDatesTests
    {
        [TestMethod]
        public void AnalyzePost_TestGetPostDate1()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija");

            Assert.AreEqual(new DateTime(2016, 11, 14, 18, 7, 0), post.DatePosted);
        }

        [TestMethod]
        public void AnalyzePost_TestGetPostDate2()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/socijalist-ili");
            Assert.AreEqual(new DateTime(2015, 4, 30, 21, 2, 0), post.DatePosted); // 30 / 04 / 2015 - 21:02
        }
        [TestMethod]
        public void AnalyzePost_TestGetPostDate3()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/kukavicje-jaje");
            Assert.AreEqual(new DateTime(2013, 6, 7, 9, 3, 0), post.DatePosted); // 07/06/2013 - 09:03
        }
        [TestMethod]
        public void AnalyzePost_TestGetPostDate4()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/che-guevarina-skola");
            Assert.AreEqual(new DateTime(2012, 10, 10, 14, 20, 0), post.DatePosted);     // 10/10/2012 - 14:20
        }
        [TestMethod]
        public void AnalyzePost_TestGetPostDate5()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/dubrovnik-dubravka-cijepanje-drva-uz-sviranje-klavira");
            Assert.AreEqual(new DateTime(2009, 6, 13, 13, 11, 0), post.DatePosted);     // 13/06/2009 - 13:11
        }
        [TestMethod]
        public void AnalyzePost_TestGetPostDate6()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/pollitika-kao-quotevo-siljim-drvo-da-ubijem-meduquot");
            Assert.AreEqual(new DateTime(2007, 12, 14, 22, 49, 0), post.DatePosted);     // 14/12/2007 - 22:49
        }
        [TestMethod]
        public void AnalyzePost_TestGetPostDate7()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/tko-drma-hac-om");
            Assert.AreEqual(new DateTime(2007, 6, 14, 18, 31, 0), post.DatePosted);     // 14/06/2007 - 18:31
        }
        [TestMethod]
        public void AnalyzePost_TestGetPostDate8()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/mesic-i-sanader-u-svadi-zbog-srpskih-izbjeglica");
            Assert.AreEqual(new DateTime(2007, 1, 6, 22, 5, 0), post.DatePosted);     // 06/01/2007 - 22:05
        }
        [TestMethod]
        public void AnalyzePost_TestGetPostDate9()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/sramim-se");
            Assert.AreEqual(new DateTime(2007, 1, 12, 22, 21, 0), post.DatePosted);     // 12/01/2007 - 22:21
        }
        [TestMethod]
        public void AnalyzePost_TestGetPostDate10()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/nered-na-trzi-tu-dobra-stvar");
            Assert.AreEqual(new DateTime(2006, 10, 13, 0, 37, 0), post.DatePosted);     // 13/10/2006 - 00:37
        }
    }
}
