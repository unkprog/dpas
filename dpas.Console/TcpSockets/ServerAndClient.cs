using dpas.Core.IO.Debug;
using dpas.Net;

namespace dpas.Console.TcpSockets
{
    public class ServerAndClient
    {
       

        public static void RunCommandLine(string[] args)
        {
            new ServerAndClient().Run();
        }

        public void Run()
        {

            new ConsoleHandler().Input((command, userparams) =>
            {
                switch (command)
                {
                    case CommandParser.Command.Exit      : Exit();                   return true;
                    case CommandParser.Command.Start     : StartServer();            return true;
                    case CommandParser.Command.Stop      : StopServer();             return true;
                    case CommandParser.Command.Connect   : ConnectClient();          return true;
                    case CommandParser.Command.Disconnect: DisconnectClient();       return true;
                    case CommandParser.Command.Send      : SendToServer(userparams); return true;
                    case CommandParser.Command.Help      : Help();                   return true;
                }
                return false;
            });
        }

        private async void Exit()
        {
            if (server != null)
            {
                bool stopped = await server.StopAsync();
                server.Dispose();
                server = null;
            }
        }

        private TcpServer server;
        private void StartServer()
        {
            System.Console.WriteLine("Command begin: Start server");
            if (server == null)
            {
                server = new TcpServer();
                server.Settings.IsLogging = true;
                server.OnReceive += (o, e) =>
                {
                    if (e.BytesTransferred > 0 && e.Socket.Connected)
                    {
                        var sendTask = server.SendAsync(e.ToArray(), e.Socket);
                    }
                };
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
