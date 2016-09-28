﻿using System.Text;
using dpas.Core.IO;
using System.IO;

namespace dpas.Service
{
    public partial class Server
    {
        internal static string serverDirectory   = AssemblyExt.GetCurrentDirectory();
        internal static string settingsDirectory = serverDirectory + "\\Settings";
        internal static string settingsFile      = settingsDirectory + "\\server.cfg";

        static byte[] DPAS = Encoding.UTF8.GetBytes("DPAS");


        internal static void CreateDirectories()
        {
            if (!Directory.Exists(settingsDirectory))
                Directory.CreateDirectory(settingsDirectory);
        }
    }
}
