using dpas.Console.TcpSockets;
using dpas.Core.IO.Debug;

namespace dpas.Console
{
    public class Program
    {
        private static void Help()
        {
            System.Console.WriteLine("Available Commands:");
            System.Console.WriteLine("tcp    = Run console Tcp server and client test");
            System.Console.WriteLine("server = Run console DPAS Server");
        }
        public static void Main(string[] args)
        {
            LogConsole.Setup();

            new ConsoleHandler().Input((command, userparams) =>
            {
                switch (command)
                {
                    case CommandParser.Command.Tcp   : ServerAndClient.RunCommandLine(args); return true;
                    case CommandParser.Command.Server: Server.RunCommandLine(args); return true;
                    case CommandParser.Command.Help  : Help(); return true;
                }
                return false;
            });
        }

    }
}
