using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ScrapySharp.Core;
using ScrapySharp.Html.Parsing;
using ScrapySharp.Network;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Html.Forms;

namespace TestLoginWithScrapy
{
    class Program
    {
        static void Main(string[] args)
        {
            // setup the browser
            ScrapingBrowser Browser = new ScrapingBrowser();
            Browser.AllowAutoRedirect = true; // Browser has many settings you can access in setup
            Browser.AllowMetaRedirect = true;
            
            //go to the home page
            WebPage PageResult = Browser.NavigateToPage(new Uri("http://www.pollitika.com"));
           
            // get first piece of data, the page title
            //HtmlNode TitleNode = PageResult.Html.CssSelect(".navbar-brand").First();
            //string PageTitle = TitleNode.InnerText;
            
            //// get a list of data from a table
            //List<String> Names = new List<string>();
            //var Table = PageResult.Html.CssSelect("#PersonTable").First();
            //foreach (var row in Table.SelectNodes("tbody/tr"))
            //{
            //    foreach (var cell in row.SelectNodes("td[1]"))
            //    {
            //        Names.Add(cell.InnerText);
            //    }
            //}
            
            // find a form and send back data
            PageWebForm form = PageResult.FindFormById("user-login-form");
            // assign values to the form fields
            form["name"] = "Zvone Radikalni";
            form["pass"] = "economist0";
            form.Method = HttpVerb.Post;
            WebPage resultsPage = form.Submit();

            PageResult = Browser.NavigateToPage(new Uri("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija"));

            Console.WriteLine(PageResult.Html.InnerHtml);
        }
    }
}
