﻿using System;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pollitika.com_Analyzer;

namespace pollitika.com_Analyzer_Tests
{
    [TestClass]
    public class AnalyzePostsTests
    {
        [TestMethod]
        public void AnalyzePost_TestPostAttributes1()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija");

            Assert.AreEqual(15395, post.Id);
            Assert.AreEqual("Hrvatsko zdravstvo i sovjetska automobilska industrija", post.Title);
            Assert.AreEqual("/node/15397/who_voted", post.VotesLink);
            Assert.AreEqual("žaki", post.Author.Name);
            Assert.AreEqual("zaki", post.Author.NameHtml);
            Assert.AreEqual(13, post.NumCommentsScrapped);
            Assert.AreEqual(24, post.GetNumberOfVotes());
            Assert.AreEqual(13, post.GetNumberOfComments());
            Assert.AreEqual(new DateTime(2016, 11, 14, 18, 7, 0), post.DatePosted);
        }

        [TestMethod]
        public void AnalyzePost_TestExtractAuthor1()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument = htmlWeb.Load("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija");

            string author, authorHtml;
            AnalyzePosts.GetPostAuthor(htmlDocument.DocumentNode.Descendants().Single(n => n.GetAttributeValue("class", "").Equals("breadcrumb")), out author, out authorHtml);

            Assert.AreEqual("žaki", author);
            Assert.AreEqual("zaki", authorHtml);
        }
    }
}
