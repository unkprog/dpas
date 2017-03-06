using System;

namespace dpas.Service.Project
{
    public partial class Project
    {
        public class Exception : Core.Exception
        {
            public Exception(int errorCode, params string[] listParams) : base(errorCode, GetErrorText(errorCode, listParams))
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
            /// <summary>
            /// Элемент проекта <{0}> не найден
            /// </summary>
            public const int ItemNotFound = AlreadyExists - 1;
            /// <summary>
            /// Для элемента проекта не указано имя
            /// </summary>
            public const int ItemEmptyName = ItemNotFound - 1;
            /// <summary>
            /// Элемент проекта <{0}> уже существует
            /// </summary>
            public const int ItemAlreadyExists = ItemEmptyName - 1;

            /// <summary>
            /// Каталог проекта <{0}> уже существует
            /// </summary>
            public const int CatalogAlreadyExists = ItemAlreadyExists - 1;

            private static string GetErrorText(int errorCode, params string[] listParams)
            {
                switch (errorCode)
                {
                    case NotSelected: return "Проект не выбран";
                    case NotFound: return string.Concat("Проект ", listParams[0], " не найден.");
                    case ArgumentNull: return "Не задана ссылка на проект";
                    case EmptyName: return "Для ссылки на проект не указано имя";
                    case AlreadyExists: return string.Concat("Проект ", listParams[0], " уже существует");
                    case ItemNotFound: return string.Concat("Элемент проекта ", listParams[0], " не найден.");
                    case ItemEmptyName: return "Для элемента проекта не указано имя";
                    case ItemAlreadyExists: return string.Concat("Элемент проекта ", listParams[0], " уже существует");
                    case CatalogAlreadyExists: return string.Concat("Каталог проекта ", listParams[0], " уже существует");
                }
                return "Проект: Неопознанная ошибка";
            }

            #endregion
        }
    }
}
