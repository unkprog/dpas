using System;

namespace dpas.Core.Data
{
    /// <summary>
    /// Перечисление состояний объекта
    /// </summary>
    public enum ObjectState
    {
        /// <summary>
        /// Объект создан
        /// </summary>
        Created = 0,
        /// <summary>
        /// Объект создан автоматически
        /// </summary>
        AutoCreated = 1,
        /// <summary>
        /// Объект изменен
        /// </summary>
        Modified = 2,
        /// <summary>
        /// Объект удален
        /// </summary>
        Deleted = 3,
        /// <summary>
        /// Нормальное состояние объекта
        /// </summary>
        Normal = 4
    }
}
