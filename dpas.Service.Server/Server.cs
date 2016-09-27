using dpas.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dpas.Service
{
    public partial class Server
    {
        public Server()
        {
        }

        private static Dictionary<string, Func<HttpRequest, IHttpHandler>> httpHandlers = new Dictionary<string, Func<HttpRequest, IHttpHandler>>();

        public void MapPath(string path, Func<HttpRequest, IHttpHandler> handler)
        {
            if (httpHandlers.ContainsKey(path))
                httpHandlers[path] = handler;
            else
                httpHandlers.Add(path, handler);
        }

        private void Initialize()
        {
            //HttpHandlerFile.SetPathSources(Environment.CurrentDirectory + "\\Server\\res");
            //ReadSettings();
         }


        private void ReadSettings()
        {
            
        }

        private void SaveSettings()
        {
        }


        private static IHttpHandler GetHttpHandler(HttpRequest request)
        {
            IHttpHandler result = null;
            Func<HttpRequest, IHttpHandler> createHandler = null;
            if (httpHandlers.TryGetValue(request.Path, out createHandler))
                result = createHandler(request);
            else
                result = new HttpHandlerFile(request);
            return result;
        }
    }
}
