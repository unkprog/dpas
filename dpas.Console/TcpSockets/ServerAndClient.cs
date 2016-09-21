using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public static Command ParseCommand(string commandstring)
        {
            string[] parts = commandstring.Split(' ');

            if (!string.IsNullOrEmpty(parts[0]))
            {
                string cmd = parts[0];

                switch (cmd.ToLower())
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
            }

            return Command.Undefined;
        }

        public static void RunCommandLine(string[] args)
        {
            new ServerAndClient().Run();
        }

        public void Run()
        { 
           
            while (!isExit)
            {
                string userinput = System.Console.ReadLine();
                if (!string.IsNullOrEmpty(userinput))
                {
                    switch (ParseCommand(userinput))
                    {
                        case Command.Exit: Exit(); break;
                        case Command.Start: StartServer(); break;
                        case Command.Stop: StopServer(); break;
                        case Command.Connect: ConnectClient(); break;
                        case Command.Disconnect: DisconnectClient(); break;
                        case Command.Send: SendToServer(); break;
                        case Command.Help: Help(); break;
                    }
                }
            }

        }

        bool isExit = false;
        private void Exit()
        {
            isExit = true;
        }

        private void StartServer()
        {

        }

        private void StopServer()
        {

        }


        private void ConnectClient()
        {

        }

        private void DisconnectClient()
        {

        }

        private void SendToServer()
        {

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
