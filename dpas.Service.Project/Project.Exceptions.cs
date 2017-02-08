using System;

namespace dpas.Service.Project
{
    public partial class Project
    {
        public class ErrorException : Core.Exception
        {
            public ErrorException(int errorCode, params string[] listParams) : base(errorCode, GetErrorText(errorCode, listParams))
            {
            }

            #region Коды ошибок

            /// <summary>
            ///  Ошибка
            /// </summary>
            public const int Error = -10001;
            /// <summary>
            /// Проект не выбран
            /// </summary>
            public const int NotSelected = Error - 1;
            /// <summary>
            ///  Проект <{0}> не найден
            /// </summary>
            public const int NotFound = NotSelected - 1;
            /// <summary>
            /// Не задана ссылка на проект
            /// </summary>
            public const int ArgumentNull = NotFound - 1;
            /// <summary>
            /// Для ссылки на проект не указано имя
            /// </summary>
            public const int EmptyName = ArgumentNull - 1;
            /// <summary>
            /// Проект <{0}> уже существует
            /// </summary>
            public const int AlreadyExists = EmptyName - 1;

            private static string GetErrorText(int errorCode, params string[] listParams)
            {
                switch (errorCode)
                {
                    case NotSelected: return "Проект не выбран";
                    case NotFound: return string.Concat("Проект ", listParams[0], " не найден.");
                    case ArgumentNull: return "Не задана ссылка на проект";
                    case EmptyName: return "Для ссылки на проект не указано имя";
                    case AlreadyExists: return string.Concat("Проект ", listParams[0], " уже существует");
                }
                return "Проект: Неопознанная ошибка";
            }

            #endregion
        }
    }
}
