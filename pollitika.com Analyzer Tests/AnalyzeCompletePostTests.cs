using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pollitika.com_Analyzer;
using pollitika.com_Data;
using pollitika.com_Model;

namespace pollitika.com_Analyzer_Tests
{
    [TestClass]
    public class AnalyzeCompletePostTests
    {
        IModelRepository _repo = new ModelRepository();

        [TestMethod]
        public void AnalyzePost_TestPostAttributes1()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija", _repo);

            // testing post attributes
            Assert.AreEqual(15397, post.Id);
            Assert.AreEqual("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija", post.HrefLink);
            //Assert.AreEqual("Hrvatsko zdravstvo i sovjetska automobilska industrija", post.Title);
            Assert.AreEqual("/node/15397/who_voted", post.VotesLink);
            Assert.AreEqual("žaki", post.Author.Name);
            Assert.AreEqual("zaki", post.Author.NameHtml);
            Assert.AreEqual(13, post.NumCommentsScrapped);
            Assert.AreEqual(24, post.GetNumberOfVotes());
            Assert.AreEqual(13, post.GetNumberOfComments());
            Assert.AreEqual(new DateTime(2016, 11, 14, 18, 7, 0), post.DatePosted);

            // testing votes
            Assert.AreEqual("ppetra", post.Votes[0].ByUser.Name);
            Assert.AreEqual(new DateTime(2016, 12, 8, 22, 53, 0), post.Votes[0].DatePosted); 
            Assert.AreEqual(1, post.Votes[0].UpOrDown);

            Assert.AreEqual("magarac", post.Votes[6].ByUser.Name);
            Assert.AreEqual(new DateTime(2016, 11, 16, 10, 52, 0), post.Votes[6].DatePosted);
            Assert.AreEqual(1, post.Votes[6].UpOrDown);

            // testing comments
            Assert.AreEqual(13, post.Comments.Count);

            Comment comm = post.Comments[0];
            Assert.AreEqual("Skviki", comm.Author.Name);
            Assert.AreEqual(0, comm.NumScrappedVotes);
            Assert.AreEqual(522047, comm.Id);
            Assert.AreEqual(new DateTime(2016, 11, 28, 16, 16, 0), comm.DatePosted);

            comm = post.Comments[4];
            Assert.AreEqual("magarac", comm.Author.Name);
            Assert.AreEqual(2, comm.NumScrappedVotes);
            Assert.AreEqual(521866, comm.Id);
            Assert.AreEqual(new DateTime(2016, 11, 16, 11, 19, 0), comm.DatePosted);

            comm = post.Comments[6];
            Assert.AreEqual("lignja", comm.Author.Name);
            Assert.AreEqual(4, comm.NumScrappedVotes);
            Assert.AreEqual(521868, comm.Id);
            Assert.AreEqual(new DateTime(2016, 11, 16, 14, 03, 0), comm.DatePosted);

            comm = post.Comments[10];
            Assert.AreEqual("sjenka", comm.Author.Name);
            Assert.AreEqual(2, comm.NumScrappedVotes);
            Assert.AreEqual(522048, comm.Id);
            Assert.AreEqual(new DateTime(2016, 11, 28, 17, 27, 0), comm.DatePosted);

            comm = post.Comments[11];
            Assert.AreEqual("ppetra", comm.Author.Name);
            Assert.AreEqual(-1, comm.NumScrappedVotes);
            Assert.AreEqual(521867, comm.Id);
            Assert.AreEqual(new DateTime(2016, 11, 16, 12, 55, 0), comm.DatePosted);
        }

        [TestMethod]
        public void AnalyzePost_TestExtractAuthor1()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument = htmlWeb.Load("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija");

            string author, authorHtml;
            AnalyzePosts.ScrapePostAuthor(htmlDocument, out author, out authorHtml);

            Assert.AreEqual("žaki", author);
            Assert.AreEqual("zaki", authorHtml);
        }
    }
}
