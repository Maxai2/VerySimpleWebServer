using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using VerySimpleWebServer.Server;

namespace VerySimpleWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            MyWebServer webServer = new MyWebServer("127.0.0.1", 5600);

            webServer.Start();

            Console.WriteLine("Web Server Start...");

            try
            {
                webServer.Listen();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Web server stopped...");
                Console.WriteLine(ex.Message);
                webServer.Stop();
            }
        }
    }
}
