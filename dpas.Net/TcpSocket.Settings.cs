using dpas.Core.IO.Settings;
using dpas.Core.Extensions;

namespace dpas.Net
{
    public partial class TcpSocket
    {
        public class SocketSettings : BaseSettings
        {
            public SocketSettings(string sectionName) : base(sectionName)
            {
            }

            /// <summary>
            /// Логирование
            /// </summary>
            public bool IsLogging
            {
                get { return dictSettings.GetBool("IsLogging", false); }
                set { dictSettings["IsLogging"] = value.ToString(); }
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
                get { return 256; } // dictSettings.GetInt32("BufferSize", 1024); }
                set { dictSettings["BufferSize"] = value.ToString(); }
            }
        }
    }
}

