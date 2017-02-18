﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;
using log4net;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace pollitika.com_Analyzer
{
    public class AnalyzeUsersPosts
    {
        // return list of URLs of posts for given user
        public static List<string> GetListOfUserPosts(string userName)
        {
            ILog log = log4net.LogManager.GetLogger(typeof(Program));

            List<string> retList = new List<string>();

            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true;                       // Browser has settings you can access in setup
            Browser.AllowMetaRedirect = true;
            Browser.Encoding = Encoding.UTF8;

            // first, we have to get total number of pages
            int pageCount   = 0;
            string pageUrl1 = "http://www.pollitika.com/blog/" + userName;
            WebPage startPage = Browser.NavigateToPage(new Uri(pageUrl1));
            HtmlNode mainContent = startPage.Html.Descendants().Where(x => x.Id == "content-main").First();

            var itemlist = mainContent.Descendants().Where(n => n.GetAttributeValue("class", "").Equals("pager")).ToList();
            if (itemlist.Count > 0)
            {
                string s = itemlist[0].LastChild.PreviousSibling.InnerHtml;

                int n1 = s.IndexOf("?page=");
                int n2 = s.IndexOf("\"", n1);
                string num = s.Substring(n1 + 6, n2 - n1 - 6);

                pageCount = Convert.ToInt32(num);
            }

            log.Info("Getting posts for user " + userName + " Number of pages " + pageCount.ToString());

            int pageIndex = 0;
            while (pageIndex <= pageCount)
            {
                string pageUrl = pageIndex <= 0 ? "http://www.pollitika.com/blog/" + userName : "http://pollitika.com/blog/" + userName + "?page=" + pageIndex.ToString();

                try
                {
                    log.Debug("Doing page " + pageUrl);

                    WebPage PageResult = Browser.NavigateToPage(new Uri(pageUrl));

                    List<string> Names = new List<string>();
                    var Table = PageResult.Html.CssSelect(".node");

                    foreach (var post in Table)
                    {
                        var a = post.CssSelect(".first");

                        foreach (var row in post.SelectNodes("h1"))
                        {
                            string title = row.InnerText;
                            string html = row.InnerHtml;

                            int start = html.IndexOf("href=\"");
                            int end = html.IndexOf("\">");

                            string href = html.Substring(start + 6, end - start - 6);

                            retList.Add(href);
                        }
                    }

                    pageIndex++;
                }
                catch (Exception e)
                {
                    log.Error("Exception while getting user posts. Msg: " + e.Message);
                    break;
                }
            }

            return retList;
        }
    }
}
