using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebServerWithMiddleWare.WebServer
{
    class MyMiddleWare2 : IMiddleWare
    {
        private DelegateMiddleWareMW next;

        public MyMiddleWare2(DelegateMiddleWareMW next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data)
        {
            Console.WriteLine("enter mw2 " + context.Request.Url.AbsolutePath);

            if (next != null)
            {
                await next.Invoke(context, data);
            }

            Console.WriteLine("exit mw2");
        }
    }
}