using dpas.Core.IO.Settings;
using dpas.Core.Extensions;

namespace dpas.Net
{
    public partial class TcpClient
    {
        public class ClientSettings : BaseSettings
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

            /// <summary>
            /// Порт, по которому доступен сервер
            /// </summary>
            public int Port
            {
                get { return dictSettings.GetInt32("Port", 25555); }
                set { dictSettings["Port"] = value.ToString(); }
            }

            /// <summary>
            /// Размер буфера в байтах для чтения-записи данных сокета
            /// </summary>
            public int BufferSize
            {
                get { return dictSettings.GetInt32("BufferSize", 1024); }
                set { dictSettings["BufferSize"] = value.ToString(); }
            }

            /// <summary>
            /// Логирование
            /// </summary>
            public bool IsLogging
            {
                get { return dictSettings.GetBool("IsLogging", false); }
                set { dictSettings["IsLogging"] = value.ToString(); }
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
