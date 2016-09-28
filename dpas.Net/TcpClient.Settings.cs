using dpas.Core.IO.Settings;
using dpas.Core.Extensions;

namespace dpas.Net
{
    public partial class TcpClient
    {
        public class ClientSettings : TcpSocket.SocketSettings
        {
            public ClientSettings() : base("Client")
            {

            }

            /// <summary>
            /// Адрес, по которому доступен сервер
            /// </summary>
            public string Server
            {
                get { return dictSettings.GetString("Server", "localhost"); }
                set { dictSettings["Server"] = value; }
            }
        }

        public ClientSettings Settings { get; private set; } = new ClientSettings();

        public void SaveSettings()
        {
            Settings.Save(System.IO.Directory.GetCurrentDirectory() + "/client.cfg");
        }

        public void ReadSettings()
        {
            Settings.Read(System.IO.Directory.GetCurrentDirectory() + "/client.cfg");
        }
    }
}
