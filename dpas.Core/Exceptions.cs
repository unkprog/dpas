using System;


namespace dpas.Core
{
    public class Exception : System.Exception
    {
        public Exception(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Код ошибки
        /// </summary>
        public int ErrorCode { get; }

        public override string ToString()
        {
            string result = string.Concat("Код ошибки:", ErrorCode, Environment.NewLine, "Текст ошибки:", Environment.NewLine, Message);
            if (!string.IsNullOrEmpty(StackTrace))
                result = string.Concat(Environment.NewLine, Environment.NewLine, "Трассировка стека:", Environment.NewLine, StackTrace);
            if (InnerException != null)
                result = string.Concat(Environment.NewLine, "Внутреннее исключение:", Environment.NewLine, InnerException.ToString());
            return result;
        }

    }
}
