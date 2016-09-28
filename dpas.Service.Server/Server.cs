using System;
using dpas.Core;
using dpas.Net;

namespace dpas.Service
{
    public partial class Server : Disposable, IServer
    {
        private TcpServer server;
        public Server()
        {
            server = new TcpServer();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (server != null)
                    server.Dispose();
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
