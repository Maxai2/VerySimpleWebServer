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
using WebServerWithMiddleWare.WebServer;

namespace WebServerWithMiddleWare.Server
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

        private DelegateMiddleWareMW firstMiddleWare;

        public MyWebServer Configure<T>() where T : IConfigurator, new() // constructor by default
        {
            IConfigurator configurator = new T();
            MiddleWareBuilder builder = new MiddleWareBuilder();

            configurator.ConfigureMiddleWare(builder);
            firstMiddleWare = builder.Build();
            return this;
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

        public async void Listen()
        {
            while (true)
            {
                HttpListenerContext context = listener.GetContext();

                MiddleWareBuilder builder = new MiddleWareBuilder();
                //builder.Use<MyMiddleWare1>().Use<MyMiddleWare2>().Use<DynamicMiddleWare>();

                //await builder.Build().Invoke(context, new Dictionary<string, object>());

                //var first = builder.Build();

                await firstMiddleWare.Invoke(context, new Dictionary<string, object>());

                // In MiddleWareBuilder
                //DynamicMiddleWare dmw = new DynamicMiddleWare(null);
                //MyMiddleWare2 mw2 = new MyMiddleWare2(dmw.InvokeAsync);
                //MyMiddleWare1 mw1 = new MyMiddleWare1(mw2.InvokeAsync);

                //await mw1.InvokeAsync(context, new Dictionary<string, object>());

                context.Response.Close();
            }
        }
    }
}
