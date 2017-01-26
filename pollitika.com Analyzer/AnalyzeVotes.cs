using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace pollitika.com_Analyzer
{
    public class ScrappedVote
    {
        public string userNick;
        public int voteValue;
        public DateTime datePosted;
    }

    public class AnalyzeVotes
    {
        public static List<ScrappedVote> ScrapeListVotesForPost(int nodeID)
        {
            List<ScrappedVote> listVotes = new List<ScrappedVote>();

            string href = "http://pollitika.com/node/" + nodeID.ToString() + "/who_voted";

            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument htmlDocument = htmlWeb.Load(href);
            var voteList = htmlDocument.DocumentNode.Descendants().Where(n => n.GetAttributeValue("class", "").Equals("view-content"));

            var content = voteList.First();
            var table = content.SelectNodes("table");

            if (table == null) // it means there is no table with votes
                return listVotes;

            var tList = table[0].ChildNodes[3]; // .ChildNodes[3]; // ("tbody");

            foreach (HtmlNode row in tList.SelectNodes("tr"))
            {
                var rowCels = row.SelectNodes("th|td");

                ScrappedVote newVote = new ScrappedVote();

                newVote.userNick = rowCels[0].InnerText.Substring(13).TrimEnd();

                string value = rowCels[1].InnerText.Substring(13).TrimEnd();
                newVote.voteValue = Convert.ToInt32(value);

                string time = rowCels[2].InnerText.Substring(13).TrimEnd();
                newVote.datePosted = Utility.ExtractDateTime(time);

                listVotes.Add(newVote);
            }
            return listVotes;
        } 
    }
}
