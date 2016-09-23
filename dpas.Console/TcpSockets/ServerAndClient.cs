using dpas.Core.IO.Debug;
using dpas.Net;

namespace dpas.Console.TcpSockets
{
    public class ServerAndClient
    {
        public enum Command
        {
            Exit,
            Start,
            Stop,
            Connect,
            Disconnect,
            Send,
            Help,
            Undefined
        }


        // Parse a command from a string
        public static Command ParseCommand(string command, out string paramstring)
        {
            string cmd, commandstring = command.Trim();
            int indexcommand = commandstring.IndexOf(' ');

            cmd = indexcommand > -1 ? commandstring.Substring(0, indexcommand).ToLower() : commandstring.ToLower();
            paramstring = indexcommand > -1 && commandstring.Length > indexcommand ? commandstring.Substring(indexcommand+ 1, commandstring.Length - indexcommand - 1) : string.Empty;

            switch (cmd)
            {
                case "exit": return Command.Exit;
                case "start": return Command.Start;
                case "stop": return Command.Stop;
                case "connect": return Command.Connect;
                case "disconnect": return Command.Disconnect;
                case "send": return Command.Send;
                case "?":
                case "help": return Command.Help;
                default: return Command.Undefined;
            }

            return Command.Undefined;
        }

        public static void RunCommandLine(string[] args)
        {
            LogConsole.Setup();
            new ServerAndClient().Run();
        }

        public void Run()
        { 
           
            while (!isExit)
            {
                string userinput = System.Console.ReadLine();
                if (!string.IsNullOrEmpty(userinput))
                {
                    string userparams = string.Empty;
                    switch (ParseCommand(userinput, out userparams))
                    {
                        case Command.Exit: Exit(); break;
                        case Command.Start: StartServer(); break;
                        case Command.Stop: StopServer(); break;
                        case Command.Connect: ConnectClient(); break;
                        case Command.Disconnect: DisconnectClient(); break;
                        case Command.Send: SendToServer(userparams); break;
                        case Command.Help: Help(); break;
                    }
                }
            }

        }

        bool isExit = false;
        private async void Exit()
        {
            if (server != null)
            {
                bool stopped = await server.StopAsync();
                server.Dispose();
                server = null;
            }
            isExit = true;
        }

        private TcpServer server;
        private void StartServer()
        {
            System.Console.WriteLine("Command begin: Start server");
            if (server == null)
            {
                server = new TcpServer();
                server.Settings.IsLogging = true;
            }
            var task = server.StartAsync();
            System.Console.WriteLine("Command end: Start server");
        }

        private void StopServer()
        {
            System.Console.WriteLine("Command begin: Stop server");
            if (server == null)
            {
                System.Console.WriteLine("Server not started");
            }
            var task = server.StopAsync();
            System.Console.WriteLine("Command end: Stop server");
        }

        private TcpClient client;
        private void ConnectClient()
        {
            System.Console.WriteLine("Command begin: Connect to server");
            if (client == null)
            {
                client = new TcpClient();
                client.Settings.IsLogging = true;
            }
            var task = client.ConnectAsync();
            System.Console.WriteLine("Command end: Connect to server");
        }

        private void DisconnectClient()
        {
            System.Console.WriteLine("Command begin: Disconnect client");
            if (client == null)
            {
                System.Console.WriteLine("Client not connected");
            }
            var task = client.DisconnectAsync();
            System.Console.WriteLine("Command end: Disconnect client");
        }

        private void SendToServer(string userparams)
        {
            System.Console.WriteLine("Command begin: Send to server");
            if (client == null)
            {
                System.Console.WriteLine("Client not connected");
                return;
            }
            string message = string.IsNullOrEmpty(userparams) ? "Test message to send server." : userparams;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            var task = client.SendAsync(data);
            System.Console.WriteLine("Command end: Send to server");
        }



        private void Help()
        {
            System.Console.WriteLine("Available Commands:");
            System.Console.WriteLine("start          = Start the server");
            System.Console.WriteLine("stop           = Stop the server");
            System.Console.WriteLine("connect        = Connect the client to the server");
            System.Console.WriteLine("disconnect     = Disconnect client from the server");
            System.Console.WriteLine("send <message> = Send a message to the server");
            System.Console.WriteLine("exit           = Stop the server and quit the program");

        }
    }
}
