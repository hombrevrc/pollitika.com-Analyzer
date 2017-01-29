using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using pollitika.com_Model;
using ScrapySharp.Extensions;

namespace pollitika.com_Analyzer
{

    public class AnalyzeVotes
    {
        public static List<Vote> ScrapeListVotesForNode(int nodeID, IModelRepository inRepo)
        {
            List<Vote> listVotes = new List<Vote>();

            string href = "http://pollitika.com/node/" + nodeID.ToString() + "/who_voted";

            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument htmlDocument = htmlWeb.Load(href);

            // najprije, da vidimo da li je samo jedna stranica s glasovima ili ih ima više
            var itemlist = htmlDocument.DocumentNode.Descendants().Where(n => n.GetAttributeValue("class", "").Equals("pager"));

            int pageCount = 1;
            if (itemlist.Count() > 0)
            {
                pageCount = itemlist.First().ChildNodes.Count/2 - 2;
            }

            for (int i = 0; i < pageCount; i++)
            {
                var voteList =
                    htmlDocument.DocumentNode.Descendants()
                        .Where(n => n.GetAttributeValue("class", "").Equals("view-content"));

                var content = voteList.First();
                var table = content.SelectNodes("table");

                if (table == null)              // it means there is no table with votes
                    return listVotes;

                var tList = table[0].ChildNodes[3];         // picking up tbody

                foreach (HtmlNode row in tList.SelectNodes("tr"))
                {
                    var rowCels = row.SelectNodes("th|td");

                    Vote newVote = new Vote();

                    string userNick = rowCels[0].InnerText.Substring(13).TrimEnd();
                    // check if user exists, add him if not
                    User user = inRepo.GetUserByName(userNick);
                    if (user == null)
                    {
                        user = new User { Name = userNick, NameHtml = userNick };
                        inRepo.AddUser(user);
                    }

                    newVote.ByUser = user;

                    string value = rowCels[1].InnerText.Substring(13).TrimEnd();
                    newVote.UpOrDown = Convert.ToInt32(value);

                    string time = rowCels[2].InnerText.Substring(13).TrimEnd();
                    newVote.DatePosted = Utility.ExtractDateTime(time);

                    listVotes.Add(newVote);
                }

                // reinicijaliziramo učitani HTML za sljedeću stranicu
                if (i < pageCount-1)
                {
                    href = "http://pollitika.com/node/" + nodeID.ToString() + "/who_voted?page=" + (i+1).ToString();

                    htmlDocument = htmlWeb.Load(href);
                }
            }
            return listVotes;
        } 
    }
}
