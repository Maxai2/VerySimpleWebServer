using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServerWithMiddleWare.WebServer
{
    public class MiddleWareBuilder
    {
        private Stack<Type> types = new Stack<Type>();

        public MiddleWareBuilder Use<T>()
        {
            // проверки на middleWare Type    
            types.Push(typeof(T));
            return this;
        }

        public DelegateMiddleWareMW Build() //builder.Use<Type1>().Use<Type2>().Use<Type3>();
        {
            DelegateMiddleWareMW first = null;

            while (types.Count > 0)
            {
                IMiddleWare mw = Activator.CreateInstance(types.Pop(), first) as IMiddleWare; // middleWare object

                first = mw.InvokeAsync;
            }

            return first;
        }
    }
}