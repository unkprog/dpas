using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dpas.Console
{
    interface IConsoleHandler
    {
        void Input(Func<CommandParser.Command, string, bool> handler);
    }

    public class ConsoleHandler: IConsoleHandler
    {
        public void Input(Func<CommandParser.Command, string, bool> handler)
        {
            bool isExit = false;
            while (!isExit)
            {
                //System.Console.Write("> ");
                string userinput = System.Console.ReadLine();
                if (!string.IsNullOrEmpty(userinput))
                {
                    string userparams = string.Empty;
                    CommandParser.Command command = CommandParser.ParseCommand(userinput, out userparams);
                    switch (command)
                    {
                        case CommandParser.Command.Exit:
                            {
                                handler?.Invoke(command, userparams);
                                isExit = true;
                                break;
                            }
                        default:
                            {
                                if (handler == null || !handler(command, userparams))
                                    System.Console.WriteLine("Undefined command.");
                                break;
                            }
                    }
                }
            }
        }
    }
}
