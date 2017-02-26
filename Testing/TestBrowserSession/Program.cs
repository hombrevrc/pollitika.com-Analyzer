using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBrowserSession
{
    class Program
    {
        static void Main(string[] args)
        {
            //initial "Login-procedure"
            BrowserSession b = new BrowserSession();
            b.Get("http://www.pollitika.com");
            b.FormElements["name"] = "Zvone Radikalni";
            b.FormElements["pass"] = "economist0";
            string response = b.Post("http://www.pollitika.com");

            response = b.Get("http://pollitika.com/hrvatsko-zdravstvo-i-sovjetska-automobilska-industrija");

            Console.WriteLine(response);
        }
    }
}
