using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using pollitika.com_AnalyzerLib;

namespace pollitika.com_Analyzer_Tests
{
    [TestClass]
    public class AnalyzeUserPostsTests
    {
        [TestMethod]
        public void GetUserPostList_Test1()
        {
            var listPosts = UserPostsAnalyzer.GetListOfUserPosts("zvone-radikalni");

            Assert.AreEqual(14 * 10 + 1, listPosts.Count);
        }
        [TestMethod]
        public void GetUserPostList_Test2()
        {
            var listPosts = UserPostsAnalyzer.GetListOfUserPosts("mrak");

            Assert.AreEqual(31 * 10 + 7, listPosts.Count);
        }

        [TestMethod]
        public void GetUserPostList_Test3()
        {
            var listPosts = UserPostsAnalyzer.GetListOfUserPosts("frederik");

            Assert.AreEqual(31 * 10 + 6, listPosts.Count);
        }
        [TestMethod]
        public void GetUserPostList_Test4()
        {
            var listPosts = UserPostsAnalyzer.GetListOfUserPosts("ppetra");

            Assert.AreEqual(25 * 10 + 10, listPosts.Count);
        }
        [TestMethod]
        public void GetUserPostList_Test5()
        {
            var listPosts = UserPostsAnalyzer.GetListOfUserPosts("otpisani");

            Assert.AreEqual(4 * 10 + 1, listPosts.Count);
        }
        [TestMethod]
        public void GetUserPostList_Test6()
        {
            var listPosts = UserPostsAnalyzer.GetListOfUserPosts("liberty-valance");

            Assert.AreEqual(0, listPosts.Count);
        }
        [TestMethod]
        public void GetUserPostList_Test7()
        {
            var listPosts = UserPostsAnalyzer.GetListOfUserPosts("zoran-ostric");

            Assert.AreEqual(45 * 10 + 6, listPosts.Count);
        }




    }
}
