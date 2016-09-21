using dpas.Core.IO.Settings;
using dpas.Core.Extensions;

namespace dpas.Net
{
    public partial class TcpServer
    {
        public class ServerSettings : BaseSettings
        {
            public ServerSettings() : base("Server")
            {

            }

            /// <summary>
            /// Порт, по которому будет доступен сервер
            /// </summary>
            public int Port
            {
                get { return dictSettings.GetInt32("Port", 25555); }
                set { dictSettings["Port"] = value.ToString(); }
            }

            /// <summary>
            /// Максимальное число подключений, которое может обрабатывать сервер
            /// </summary>
            public int MaxConnections
            {
                get { return dictSettings.GetInt32("MaxConnections", 100); }
                set { dictSettings["MaxConnections"] = value.ToString(); }
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
