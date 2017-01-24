using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace pollitika.com_Analyzer
{
    public class AnalyzeVotes
    {
        public static List<Vote> ScrapeListVotesForPost(int nodeID)
        {
            List<Vote> listVotes = new List<Vote>();

            string href = "http://pollitika.com/node/" + nodeID.ToString() + "/who_voted";

            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument htmlDocument = htmlWeb.Load(href);
            HtmlNode voteList = htmlDocument.DocumentNode.Descendants().Single(n => n.GetAttributeValue("class", "").Equals("views-table cols-24"));

            var table = voteList.SelectNodes("tbody");

            foreach (HtmlNode row in table[0].SelectNodes("tr"))
            {
                var rowCels = row.SelectNodes("th|td");

                string user = rowCels[0].InnerText.Substring(13).TrimEnd();
                string value = rowCels[1].InnerText.Substring(13).TrimEnd();
                string time = rowCels[2].InnerText.Substring(13).TrimEnd();

                Vote newVote = new Vote();
            }
            return listVotes;
        } 
    }
}
