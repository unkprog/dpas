using dpas.Core.Extensions;

namespace dpas.Net
{
    public partial class TcpServer
    {
        public class ServerSettings : TcpSocket.SocketSettings
        {
            public ServerSettings() : base("Server")
            {

            }

            /// <summary>
            /// Максимальное число подключений, которое может обрабатывать сервер
            /// </summary>
            public int MaxConnections
            {
                get { return dictSettings.GetInt32("MaxConnections", 100); }
                set { dictSettings["MaxConnections"] = value.ToString(); }
            }
        }

        public ServerSettings Settings { get; private set; } = new ServerSettings();

        public void SaveSettings()
        {
            Settings.Save(System.IO.Directory.GetCurrentDirectory() + "/server.cfg");
        }

        public void ReadSettings()
        {
            Settings.Read(System.IO.Directory.GetCurrentDirectory() + "/server.cfg");
        }
    }
}
