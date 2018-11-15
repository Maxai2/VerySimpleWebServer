using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServerWithMiddleWare.WebServer
{
    public interface IConfigurator
    {
        void ConfigureMiddleWare(MiddleWareBuilder builder);
    }

    public class Configurator : IConfigurator
    {
        public void ConfigureMiddleWare(MiddleWareBuilder builder)
        {
            builder.Use<MyMiddleWare1>().Use<MyMiddleWare2>().Use<DynamicMiddleWare>();
        }
    }
}
