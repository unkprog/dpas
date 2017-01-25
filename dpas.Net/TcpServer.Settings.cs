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
                get { return dictSettings.GetInt32("MaxConnections", 10); }
                set { dictSettings["MaxConnections"] = value.ToString(); }
            }
        }

        public ServerSettings Settings { get; private set; } = new ServerSettings();
    }
}
