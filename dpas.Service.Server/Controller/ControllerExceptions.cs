using System;

namespace dpas.Service.Controller.Api
{
    public static class ErrorCode
    {
        #region Command
        /// <summary>
        /// Константы ошибок для проекта (старт -1)
        /// </summary>
        public static class Command
        {
            // Ошибка
            public const int Error = -1;
            // Команда <{0}> не поддерживается
            public const int IsNotSupported = Error - 1;

            // Ошибка при выполнении команды <{command}>: {0}
            public const int CommandFailed = IsNotSupported - 1;


            public const int LastErrorCode = IsNotSupported - 1;
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
        #endregion

 
    }
}
