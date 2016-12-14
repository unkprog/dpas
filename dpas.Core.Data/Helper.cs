
namespace dpas.Core.Data
{
    public static class Helper
    {
        /// <summary>
        /// Список символов для исключения
        /// </summary>
        public static readonly char[] EscapeChars = new char[] { ')', '(', ' ', '.', ',', '"', '\'', '/', '\\', '-', '&', '!', '#', '$', '%', '^', '?', ':', ';', '*', '+', '=', '№', '<', '>', '{', '}', '@', '[', ']', (char)160 };

        /// <summary>
        /// Замена символов-исключений на символ подчеркивания ('_')
        /// </summary>
        /// <param name="aValue">Строка, в которой необходимо заменить исключаемые символы</param>
        /// <returns>Результат после замены символов-исключений</returns>
        public static string ReplaceEscapeChars(string aValue)
        {
            string result = string.IsNullOrEmpty(aValue) ? string.Empty : aValue;
            for (int i = 0, icount = EscapeChars.Length; i < icount; i++)
                result = result.Replace(EscapeChars[i], '_');
            return result;
        }

        /// <summary>
        /// Получение имени свойства для строке
        /// </summary>
        /// <param name="aValue">Строка для получения имени свойства</param>
        /// <returns>Имя свойства</returns>
        public static string GetPropertyName(string aValue)
        {
            string result = ReplaceEscapeChars(aValue);
            if (string.IsNullOrEmpty(result)) result = "_tempProperty";
            char ch = result[0];
            if (ch >= '0' && ch <= '9') result = string.Concat("_", result);
            return result;
        }
    }
}
