namespace dpas.Service.Project
{
    public partial class ProjectItem
    {
        /// <summary>
        /// Проект
        /// </summary>
        public const int Project   = 0; 
        /// <summary>
        /// Справочники
        /// </summary>
        public const int Reference = 1;
        /// <summary>
        /// Данные
        /// </summary>
        public const int Data      = 2;
        /// <summary>
        /// Элемент справочника
        /// </summary>
        public const int ReferenceItem = 3;
        /// <summary>
        /// Элемент данных
        /// </summary>
        public const int DataItem = 4;



        /// <summary>
        /// Тип поля
        /// </summary>
        
        /// <summary>
        /// Строка
        /// </summary>
        public const int FieldString = 0;
        /// <summary>
        /// Целое число
        /// </summary>
        public const int FieldInt = 1;
        /// <summary>
        /// Число двойной точности с плавающей запятой
        /// </summary>
        public const int FieldDouble = 2;
        /// <summary>
        /// Логическое
        /// </summary>
        public const int FieldBool = 3;
        /// <summary>
        /// Дата
        /// </summary>
        public const int FieldDate = 4;
        /// <summary>
        /// Класс
        /// </summary>
        public const int FieldClass = 5;
    }
}
