using System.Text;
using dpas.Core.IO;

namespace dpas.Service
{
    public partial class Server
    {
        internal static string serverDirectory   = AssemblyExt.GetCurrentDirectory() + "\\Server";
        internal static string settingsDirectory = serverDirectory + "\\Server\\settings";
        internal static string settingsFile      = settingsDirectory + "\\server.xml";

        static byte[] DPAS = Encoding.UTF8.GetBytes("DPAS");
    }
}
