﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ScrapySharp.Network;

using pollitika.com_AnalyzerLib;
using pollitika.com_Data;
using pollitika.com_Model;

namespace pollitika.com_Analyzer_Tests
{
    /// <summary>
    /// Summary description for AnalyzeVotesTests
    /// </summary>
    [TestClass]
    public class AnalyzeVotesTests
    {
        private IModelRepository _repo = new ModelRepository();

        public AnalyzeVotesTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void AnalyzePost_TestGetPostVotes1()
        {
            // "http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija"

            ScrapingBrowser browser = Utility.GetLoggedBrowser();

            List<Vote> listVotes = VotesAnalyzer.ScrapeListVotesForNode(15397, null, "node", _repo, browser);

            Assert.AreEqual(24, listVotes.Count);

            // testing votes
            List<string> usersWhoVoted = new List<string>
            {
                "ppetra", "leddevet", "otpisani", "Quinquaginta", "z00ey", "NolS", "magarac", "antonac", "martin", "profesor", "Elnino",
                "pravednik vz", "boltek", "marival", "lunoprof", "Skviki", "lignja", "Jura", "mario121", "hlad", "gledamokosebe", "zaphod", "indian", "Bigulica"
            };
            List<string> usersWhoVotedNicks = new List<string>
            {
                "ppetra", "leddevet", "otpisani", "quinquaginta", "z00ey", "nols", "magarac", "antonac", "martin", "profesor", "elnino",
                "pravednik-vz", "boltek", "marival", "lunoprof", "skviki", "lignja", "jura", "mario121", "hlad", "gledamokosebe", "zaphod", "indian", "bigulica"
            };

            // check all user names who voted
            for (int i = 0; i < usersWhoVoted.Count; i++)
            {
                Assert.AreEqual(usersWhoVoted[i], listVotes[i].ByUser.Name);
                Assert.AreEqual(usersWhoVotedNicks[i], listVotes[i].ByUser.NameHtml);
            }
        }

        [TestMethod]
        public void AnalyzePost_TestGetPostVotes2()
        {
            // "http://pollitika.com/socijalist-ili"

            List<Vote> listVotes = VotesAnalyzer.ScrapeListVotesForNode(14171, null, "node", _repo);

            Assert.AreEqual(22, listVotes.Count);
        }

        [TestMethod]
        public void AnalyzePost_TestGetPostVotes3_TwoPageOfVotes()
        {
            // "http://pollitika.com/che-guevarina-skola"
            List<Vote> listVotes = VotesAnalyzer.ScrapeListVotesForNode(11768, null, "node", _repo);

            // TODO - ima glasova i na sljedećoj stranici!!!!
            Assert.AreEqual(33, listVotes.Count);
        }

        [TestMethod]
        public void AnalyzePost_TestGetPostVotes4()
        {
            // "http://pollitika.com/pollitika-kao-quotevo-siljim-drvo-da-ubijem-meduquot"

            List<Vote> listVotes = VotesAnalyzer.ScrapeListVotesForNode(2898, null, "node", _repo);

            Assert.AreEqual(17, listVotes.Count);
        }

        [TestMethod]
        public void AnalyzePost_TestGetPostVotes5_NoVotes()
        {
            // "http://pollitika.com/sramim-se"

            List<Vote> listVotes = VotesAnalyzer.ScrapeListVotesForNode(406, null, "node", _repo);

            Assert.AreEqual(0, listVotes.Count);
        }

        [TestMethod]
        public void AnalyzePost_TestGetPostVotes6_NoVotes()
        {
            // "http://pollitika.com/nered-na-trzi-tu-dobra-stvar"

            List<Vote> listVotes = VotesAnalyzer.ScrapeListVotesForNode(50, null, "node", _repo);

            Assert.AreEqual(0, listVotes.Count);
        }
        [TestMethod]
        public void AnalyzePost_TestGetPostVotes7_FourPagesOfVotes()
        {
            // "http://pollitika.com/sve-sto-vam-nitko-nije-htio-rei-o-birakim-popisima"

            List<Vote> listVotes = VotesAnalyzer.ScrapeListVotesForNode(6085, null, "node", _repo);

            Assert.AreEqual(92, listVotes.Count);
        }
        [TestMethod]
        public void AnalyzePost_TestGetPostVotes8_MostVotes_100()
        {
            // "http://pollitika.com/node/8084"

            List<Vote> listVotes = VotesAnalyzer.ScrapeListVotesForNode(8084, null, "node", _repo);

            Assert.AreEqual(100, listVotes.Count);
        }
    }
}
