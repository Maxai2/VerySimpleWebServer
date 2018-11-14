using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VerySimpleWebServer.Services;

namespace VerySimpleWebServer.Server
{
    class MyWebServer
    {
        private readonly string ip;
        private readonly int port;

        private HttpListener listener;
        private BookServices bookService = new BookServices();

        public MyWebServer(string ip, int port)
        {
            this.ip = ip;
            this.port = port;

            listener = new HttpListener();

            listener.Prefixes.Add($"http://{ip}:{port}/");
        }

        public void Start()
        {
            if (listener != null)
            {
                listener.Start();
            }
        }

        public void Stop()
        {
            if (listener != null && listener.IsListening)
            {
                listener.Stop();
            }
        }

        public void Listen()
        {
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                string path = context.Request.Url.AbsolutePath;
                string method = context.Request.HttpMethod;

                if (method == HttpMethod.Get.Method && path == "/books")
                {
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("<ol>");

                    foreach (string book in bookService.GetBooks())
                    {
                        strBuilder.Append($"<li>{book}</li>"); 
                    }

                    strBuilder.Append("</ol>");
                    strBuilder.Append("<form method='POST' action='http://127.0.0.1:5600/addbook'>" + "<label>Book: </label>" + "<input type='text' name='bookName' required>" + "<input type='submit'>" + "</form>");

                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "text/html";

                    using (StreamWriter sw = new StreamWriter(context.Response.OutputStream))
                    {
                        sw.Write(strBuilder.ToString());
                    }
                }
                else
                if (method == HttpMethod.Post.Method && path == "/addbook")
                {
                    string data;

                    // body - context.Request.InputStream
                    using (StreamReader sr = new StreamReader(context.Request.InputStream))
                    {
                        data = sr.ReadToEnd();
                    }

                    NameValueCollection res = HttpUtility.ParseQueryString(data);
                    string book = res["bookname"];
                    bookService.AddBook(book);
                    context.Response.Redirect($"http://{ip}:{port}/books");
                }
                else
                {
                    context.Response.StatusCode = 404;
                }

                context.Response.Close();
            }
        }
    }
}
