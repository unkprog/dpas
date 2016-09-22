using System;

namespace dpas.Core.IO.Debug
{
    public interface ILog
    {
        void Write(string data);
    }

    public class BaseLog : ILog
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
            BaseLog.log = new LogConsole();
        }
    }

}
