using System;


namespace dpas.Console
{
    public class Server
    {
        public static void RunCommandLine(string[] args)
        {
            new Server().Run();
        }

        public void Run()
        {
            new ConsoleHandler().Input((command, userparams) =>
            {
                switch (command)
                {
                    //case CommandParser.Command.Tcp: ServerAndClient.RunCommandLine(args); return true;
                    //case CommandParser.Command.Server: Server.RunCommandLine(args); return true;
                    case CommandParser.Command.Help: Help(); return true;
                }
                return false;
            });
        }


        private void Help()
        {
            System.Console.WriteLine("Available Commands:");
            System.Console.WriteLine("start          = Start the DPAS server");
            System.Console.WriteLine("stop           = Stop the DPAS server");
            //System.Console.WriteLine("connect        = Connect the client to the server");
            //System.Console.WriteLine("disconnect     = Disconnect client from the server");
            //System.Console.WriteLine("send <message> = Send a message to the server");
            System.Console.WriteLine("exit           = Quit console and stop DPAS server ");
        }
    }
}
