using dpas.Core;
using Microsoft.Extensions.Logging;

namespace dpas.Service
{
    public partial class Server : Disposable, IServer
    {
        private DpasTcpServer server;
        
        public Server(ILoggerFactory loggerFactory)
        {
            server = new DpasTcpServer(loggerFactory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (server != null)
                {
                    server.Dispose();
                }
                server = null;
            }
            base.Dispose(disposing);
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

       
    }
}
