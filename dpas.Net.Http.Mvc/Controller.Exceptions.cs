using System;

namespace dpas.Net.Http.Mvc
{
    public partial class Controller 
    {
        public class Exception : Core.Exception
        {
            public Exception(int errorCode, string command, params string[] listParams) : base(errorCode, GetErrorText(errorCode, command, listParams))
            {
            }


            /// <summary>
            /// Неопознанная ошибка
            /// </summary>
            public const int Error = -1;
            /// <summary>
            /// Команда <{command}> не поддерживается
            /// </summary>
            public const int IsNotSupported = Error - 1;
            /// <summary>
            /// Ошибка при выполнении команды <{command}>: {listParams[0]}
            /// </summary>
            public const int CommandFailed = IsNotSupported - 1;

            public static string GetErrorText(int errorCode, string command, params string[] listParams)
            {
                switch (errorCode)
                {
                    case IsNotSupported: return string.Concat("Команда <", command, "> не поддерживается");
                    case CommandFailed: return string.Concat("Ошибка при выполнении команды <", command, ">:", Environment.NewLine, listParams[0]);
                }
                return string.Concat("Команда <", command, ">: Неопознанная ошибка");
            }
        }
    }
}
