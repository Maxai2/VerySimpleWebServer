using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebServerWithMiddleWare.WebServer
{
    public interface IMiddleWare
    {
        //logic
        //link next MiddleWare
        //reference prev MiddleWare

        // DelegateMW next
        // ctor (DelegateMW next)

        Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data);


    }
}