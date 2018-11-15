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
using WebServerWithMiddleWare.Services;

namespace WebServerWithMiddleWare.WebServer
{
    class DynamicMiddleWare : IMiddleWare
    {
        private DelegateMiddleWareMW next;

        public DynamicMiddleWare(DelegateMiddleWareMW next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data)
        {
            Console.WriteLine("enter dynamicmw " + context.Request.Url.AbsolutePath);

            string path = context.Request.Url.AbsolutePath;
            string method = context.Request.HttpMethod;

            BookServices bookService = new BookServices();

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
                string query;

                // body - context.Request.InputStream
                using (StreamReader sr = new StreamReader(context.Request.InputStream))
                {
                    query = sr.ReadToEnd();
                }


                //context.Request.QueryString["token"];
                NameValueCollection res = HttpUtility.ParseQueryString(query);
                string book = res["bookname"];
                bookService.AddBook(book);
                context.Response.Redirect($"/books");
            }
            else
            {
                context.Response.StatusCode = 404;
            }

            if (next != null)
            {
                await next.Invoke(context, data);
            }

            Console.WriteLine("exit dynamicmw");
        }
    }
}
