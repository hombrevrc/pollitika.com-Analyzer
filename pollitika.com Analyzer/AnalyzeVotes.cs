using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using pollitika.com_Model;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace pollitika.com_Analyzer
{

    public class AnalyzeVotes
    {
        // inType: "node" - for getting votes for posts, "comment" - for getting votes for comments
        public static List<Vote> ScrapeListVotesForNode(int nodeID, User nodeAuthor, string inType, IModelRepository inRepo, ScrapingBrowser inBrowser=null)
        {
            List<Vote> listVotes = new List<Vote>();

            string href = "http://pollitika.com/" + inType + "/" + nodeID.ToString() + "/who_voted";

            HtmlNode mainNode = null;
            if (inBrowser == null)
            {
                HtmlWeb htmlWeb = new HtmlWeb();
                HtmlDocument  htmlDocument = htmlWeb.Load(href);
                mainNode = htmlDocument.DocumentNode;
            }
            else {
                WebPage PageResult = inBrowser.NavigateToPage(new Uri(href));
                mainNode = PageResult.Html;
            }

            // najprije, da vidimo da li je samo jedna stranica s glasovima ili ih ima više
            var itemlist = mainNode.Descendants().Where(n => n.GetAttributeValue("class", "").Equals("pager"));

            int pageCount = 1;
            if (itemlist.Count() > 0)
            {
                pageCount = itemlist.First().ChildNodes.Count/2 - 2;
            }

            for (int i = 0; i < pageCount; i++)
            {
                var voteList = mainNode.Descendants().Where(n => n.GetAttributeValue("class", "").Equals("view-content"));

                var content = voteList.First();
                var table = content.SelectNodes("table");

                if (table == null)              // it means there is no table with votes
                    return listVotes;

                var tList = table[0].ChildNodes[3];         // picking up tbody

                foreach (HtmlNode row in tList.SelectNodes("tr"))
                {
                    var rowCels = row.SelectNodes("th|td");

                    Vote newVote = new Vote();

                    string userName = rowCels[0].InnerText.Substring(13).TrimEnd();

                    // let's see if we can get his html nick
                    string userNick = "";
                    string str = rowCels[0].InnerHtml;
                    int usrInd = str.IndexOf("/user/");
                    if (usrInd != -1)
                    {
                        int usrInd2 = str.IndexOf("title=", usrInd);
                        userNick = str.Substring(usrInd + 6, usrInd2 - usrInd - 8);
                    }

                    // check if user exists, add him if not
                    User user = inRepo.GetUserByName(userName);
                    if (user == null)
                    {
                        user = new User { Name = userName, NameHtml = userNick };
                        inRepo.AddUser(user);
                    }

                    newVote.ByUser = user;
                    newVote.VoteForUser = nodeAuthor;

                    string value = rowCels[1].InnerText.Substring(13).TrimEnd();
                    newVote.UpOrDown = Convert.ToInt32(value);

                    string time = rowCels[2].InnerText.Substring(13).TrimEnd();
                    newVote.DatePosted = Utility.ExtractDateTime2(time);

                    listVotes.Add(newVote);
                }

                // reinicijaliziramo učitani HTML za sljedeću stranicu
                if (i < pageCount-1)
                {
                    href = "http://pollitika.com/node/" + nodeID.ToString() + "/who_voted?page=" + (i+1).ToString();

                    if (inBrowser == null)
                    {
                        HtmlWeb htmlWeb = new HtmlWeb();
                        HtmlDocument htmlDocument = htmlWeb.Load(href);
                        mainNode = htmlDocument.DocumentNode;
                    }
                    else
                    {
                        WebPage PageResult = inBrowser.NavigateToPage(new Uri(href));
                        mainNode = PageResult.Html;
                    }
                }
            }
            return listVotes;
        } 
    }
}
