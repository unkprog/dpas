namespace dpas.Console
{
    public class CommandParser
    {
        public enum Command
        {
            // Main commands
            Help,
            Tcp,
            Server,
            Exit,

            //
            Start,
            Stop,
            Connect,
            Disconnect,
            Send,
            Undefined
        }


        // Parse a command from a string
        public static Command ParseCommand(string command, out string paramstring)
        {
            string cmd, commandstring = command.Trim();
            int indexcommand = commandstring.IndexOf(' ');

            cmd = indexcommand > -1 ? commandstring.Substring(0, indexcommand).ToLower() : commandstring.ToLower();
            paramstring = indexcommand > -1 && commandstring.Length > indexcommand ? commandstring.Substring(indexcommand + 1, commandstring.Length - indexcommand - 1) : string.Empty;

            switch (cmd)
            {
                case "tcp": return Command.Tcp;
                case "server": return Command.Server;
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

            //return Command.Undefined;
        }
    }
}
