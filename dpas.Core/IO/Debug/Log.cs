using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpas.Core.IO.Debug
{
    public interface ILog
    {
        void Write(string data);
    }

    public class LogBase : ILog
    {
        internal static ILog log;

        public static void WriteToLog(string data)
        {
            if (log != null)
                log.Write(data);
        }

        public void Write(string data)
        {
            if (log != null)
                log.Write(data);
        }
    }

   

    public class LogConsole : ILog
    {
        public void Write(string data)
        {
            Console.WriteLine(data);
        }

        public static void Setup()
        {
            LogBase.log = new LogConsole();
        }
    }

}
