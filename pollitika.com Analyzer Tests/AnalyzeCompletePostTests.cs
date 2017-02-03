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
        public void AnalyzePost_CompletePostTest1()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija", _repo, true, true);

            // testing post attributes
            Assert.AreEqual(15397, post.Id);
            Assert.AreEqual("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija", post.HrefLink);
            Assert.AreEqual("Hrvatsko zdravstvo i sovjetska automobilska industrija", post.Title);
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
            Assert.AreEqual(0, comm.NumScrappedVotes);
            Assert.AreEqual(2, comm.Votes.Count);
            Assert.AreEqual("Liberty Valance", comm.Votes[0].ByUser.Name);
            Assert.AreEqual(new DateTime(2017, 01, 28, 12, 18, 0), comm.Votes[0].DatePosted);
            Assert.AreEqual(-1, comm.Votes[0].UpOrDown);
            Assert.AreEqual("Zvone Radikalni", comm.Votes[1].ByUser.Name);
            Assert.AreEqual(new DateTime(2017, 01, 28, 12, 16, 0), comm.Votes[1].DatePosted);
            Assert.AreEqual(1, comm.Votes[1].UpOrDown);

            comm = post.Comments[4];
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


            comm = post.Comments[6];
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


            // TODO - definirati votes za komentare
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
        public void AnalyzePost_CompletePostTest2()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/kukavicje-jaje", _repo, true, true);

            // testing post attributes
            Assert.AreEqual(12480, post.Id);
            Assert.AreEqual("http://pollitika.com/kukavicje-jaje", post.HrefLink);
            Assert.AreEqual("Kukavičje jaje", post.Title);
            Assert.AreEqual("/node/12480/who_voted", post.VotesLink);
            Assert.AreEqual("Rebel", post.Author.Name);
            Assert.AreEqual("rebel", post.Author.NameHtml);
            Assert.AreEqual(55, post.NumCommentsScrapped);
            Assert.AreEqual(26, post.GetNumberOfVotes());
            Assert.AreEqual(55, post.GetNumberOfComments());
            Assert.AreEqual(new DateTime(2013, 6, 7, 9, 3, 0), post.DatePosted); // 07/06/2013 - 09:03
        }

        [TestMethod]
        public void AnalyzePost_CompletePostTest3()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/trijumf-trollova", _repo, true, true);

            // testing post attributes
            Assert.AreEqual(14888, post.Id);
            Assert.AreEqual("http://pollitika.com/trijumf-trollova", post.HrefLink);
            Assert.AreEqual("Trijumf trollova", post.Title);
            Assert.AreEqual("/node/14888/who_voted", post.VotesLink);
            Assert.AreEqual("magarac", post.Author.Name);
            Assert.AreEqual("magarac", post.Author.NameHtml);
            Assert.AreEqual(159, post.NumCommentsScrapped);
            Assert.AreEqual(41, post.GetNumberOfVotes());
            Assert.AreEqual(159, post.GetNumberOfComments());
            Assert.AreEqual(new DateTime(2016, 3, 17, 17, 34, 0), post.DatePosted);
        }

        [TestMethod]
        public void AnalyzePost_CompletePostTest4()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/dubrovnik-dubravka-cijepanje-drva-uz-sviranje-klavira", _repo, true, true);

            // testing post attributes
            Assert.AreEqual(6425, post.Id);
            Assert.AreEqual("http://pollitika.com/dubrovnik-dubravka-cijepanje-drva-uz-sviranje-klavira", post.HrefLink);
            Assert.AreEqual(" Dubrovnik, Dubravka, cijepanje drva uz sviranje klavira", post.Title);
            Assert.AreEqual("/node/6425/who_voted", post.VotesLink);
            Assert.AreEqual("Marshal", post.Author.Name);
            Assert.AreEqual("marshal", post.Author.NameHtml);
            Assert.AreEqual(6, post.NumCommentsScrapped);
            Assert.AreEqual(12, post.GetNumberOfVotes());
            Assert.AreEqual(6, post.GetNumberOfComments());
            Assert.AreEqual(new DateTime(2009, 6, 13, 13, 11, 0), post.DatePosted);     // 13/06/2009 - 13:11
        }

        [TestMethod]
        public void AnalyzePost_CompletePostTest5()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/che-guevarina-skola", _repo, true, true);

            // testing post attributes
            Assert.AreEqual(11768, post.Id);
            Assert.AreEqual("http://pollitika.com/che-guevarina-skola", post.HrefLink);
            Assert.AreEqual("Che Guevarina škola", post.Title);
            Assert.AreEqual("/node/11768/who_voted", post.VotesLink);
            Assert.AreEqual("Golgota", post.Author.Name);
            Assert.AreEqual("golgota", post.Author.NameHtml);
            Assert.AreEqual(167, post.NumCommentsScrapped);
            Assert.AreEqual(33, post.GetNumberOfVotes());
            Assert.AreEqual(167, post.GetNumberOfComments());
            Assert.AreEqual(new DateTime(2012, 10, 10, 14, 20, 0), post.DatePosted);
        }

        [TestMethod]
        public void AnalyzePost_CompletePostTest6()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/sramim-se", _repo, true, true);

            // testing post attributes
            Assert.AreEqual(406, post.Id);
            Assert.AreEqual("http://pollitika.com/sramim-se", post.HrefLink);
            Assert.AreEqual("Sramim se!", post.Title);
            Assert.AreEqual("/node/406/who_voted", post.VotesLink);
            Assert.AreEqual("drlesar", post.Author.Name);
            Assert.AreEqual("drlesar", post.Author.NameHtml);
            Assert.AreEqual(16, post.NumCommentsScrapped);
            Assert.AreEqual(0, post.GetNumberOfVotes());
            Assert.AreEqual(16, post.GetNumberOfComments());
            Assert.AreEqual(new DateTime(2007, 1, 12, 22, 21, 0), post.DatePosted);
        }

        [TestMethod]
        public void AnalyzePost_CompletePostTest7()
        {
            Post post = AnalyzePosts.AnalyzePost("http://pollitika.com/nered-na-trzi-tu-dobra-stvar", _repo, true, true);

            // testing post attributes
            Assert.AreEqual(50, post.Id);
            Assert.AreEqual("http://pollitika.com/nered-na-trzi-tu-dobra-stvar", post.HrefLink);
            Assert.AreEqual("Nered na tržištu, dobra stvar", post.Title);
            Assert.AreEqual("/node/50/who_voted", post.VotesLink);
            Assert.AreEqual("Simun", post.Author.Name);
            Assert.AreEqual("simun", post.Author.NameHtml);
            Assert.AreEqual(4, post.NumCommentsScrapped);
            Assert.AreEqual(0, post.GetNumberOfVotes());
            Assert.AreEqual(4, post.GetNumberOfComments());
            Assert.AreEqual(new DateTime(2006, 10, 13, 0, 37, 0), post.DatePosted);
        }
    }
}
