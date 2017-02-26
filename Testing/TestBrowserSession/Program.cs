using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestBrowserSession
{
    class Program
    {
        static void Main(string[] args)
        {
            //initial "Login-procedure"
            //BrowserSession b = new BrowserSession();
            //b.Get("http://www.pollitika.com");
            //b.FormElements["name"] = "Zvone Radikalni";
            //b.FormElements["pass"] = "economist0";
            //string response = b.Post("http://www.pollitika.com");

            //response = b.Get("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija");

            //Console.WriteLine(response);

            string html = readHtmlPage("http://www.pollitika.com");

            Console.WriteLine(html);
        }

        private static String readHtmlPage(string url)
        {

            //setup some variables

            String username = "Zvone Radikalni";
            String password = "economist0";
            String firstname = "John";
            String lastname = "Smith";

            //setup some variables end

            String result = "";
            String strPost = "name=" + username + "&pass=" + password; // + "&firstname=" + firstname + "&lastname=" + lastname;
            StreamWriter myWriter = null;

            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Method = "POST";
            objRequest.ContentLength = strPost.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";

            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(strPost);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                myWriter.Close();
            }

            StringBuilder sb = new StringBuilder();

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr =
               new StreamReader(objResponse.GetResponseStream()))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.Append(line);
                }
                result = sb.ToString();

                // Close and clean up the StreamReader
                sr.Close();
            }
            return result;
        }
    }
}
