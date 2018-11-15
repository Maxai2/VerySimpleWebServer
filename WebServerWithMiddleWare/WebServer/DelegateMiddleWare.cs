using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebServerWithMiddleWare.WebServer
{
    public delegate Task DelegateMiddleWareMW (HttpListenerContext context, Dictionary<string, object> data);
}
