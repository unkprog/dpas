
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
                    case CommandParser.Command.Start: StartServer(); return true;
                    case CommandParser.Command.Stop : StopServer(); return true;
                    case CommandParser.Command.Help : Help(); return true;
                }
                return false;
            });
        }

        private Service.IServer server;
        private void StartServer()
        {
            System.Console.WriteLine("Command begin: Start server");
            if (server == null)
            {
                server = new Service.Server(null);
                server.Settings.IsLogging = true;
            }
            server.Start();
            System.Console.WriteLine("Command end: Start server");
        }

        private void StopServer()
        {
            System.Console.WriteLine("Command begin: Stop server");
            if (server != null)
                server.Stop();
            else
                System.Console.WriteLine("Server not started");
            System.Console.WriteLine("Command end: Stop server");
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
