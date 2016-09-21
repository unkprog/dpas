using dpas.Console.TcpSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dpas.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServerAndClient.RunCommandLine(args);
        }
    }
}
