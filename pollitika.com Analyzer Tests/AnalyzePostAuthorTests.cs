using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pollitika.com_Analyzer;

namespace pollitika.com_Analyzer_Tests
{
    [TestClass]
    public class AnalyzePostAuthorTests
    {
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
        [TestMethod]
        public void AnalyzePost_TestExtractAuthor2()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument = htmlWeb.Load("http://pollitika.com/kukavicje-jaje");

            string author, authorHtml;
            AnalyzePosts.ScrapePostAuthor(htmlDocument, out author, out authorHtml);

            Assert.AreEqual("Rebel", author);
            Assert.AreEqual("rebel", authorHtml);
        }
        [TestMethod]
        public void AnalyzePost_TestExtractAuthor3()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument = htmlWeb.Load("http://pollitika.com/pollitika-kao-quotevo-siljim-drvo-da-ubijem-meduquot");

            string author, authorHtml;
            AnalyzePosts.ScrapePostAuthor(htmlDocument, out author, out authorHtml);

            Assert.AreEqual("Mali Hans", author);
            Assert.AreEqual("mali-hans", authorHtml);
        }
        [TestMethod]
        public void AnalyzePost_TestExtractAuthor4()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument = htmlWeb.Load("http://pollitika.com/spam-modul");

            string author, authorHtml;
            AnalyzePosts.ScrapePostAuthor(htmlDocument, out author, out authorHtml);

            Assert.AreEqual("mrak", author);
            Assert.AreEqual("mrak", authorHtml);
        }
        [TestMethod]
        public void AnalyzePost_TestExtractAuthor5()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument = htmlWeb.Load("http://pollitika.com/tko-drma-hac-om");

            string author, authorHtml;
            AnalyzePosts.ScrapePostAuthor(htmlDocument, out author, out authorHtml);

            Assert.AreEqual("2bbc", author);
            Assert.AreEqual("2bbc", authorHtml);
        }
        [TestMethod]
        public void AnalyzePost_TestExtractAuthor6()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument = htmlWeb.Load("http://pollitika.com/sramim-se");

            string author, authorHtml;
            AnalyzePosts.ScrapePostAuthor(htmlDocument, out author, out authorHtml);

            Assert.AreEqual("drlesar", author);
            Assert.AreEqual("drlesar", authorHtml);
        }
        [TestMethod]
        public void AnalyzePost_TestExtractAuthor7()
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument htmlDocument = htmlWeb.Load("http://pollitika.com/nered-na-trzi-tu-dobra-stvar");

            string author, authorHtml;
            AnalyzePosts.ScrapePostAuthor(htmlDocument, out author, out authorHtml);

            Assert.AreEqual("Simun", author);
            Assert.AreEqual("simun", authorHtml);
        }
    }
}
