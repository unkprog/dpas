
namespace dpas.Core.Data.Specialization
{
    /// <summary>
    /// Базовый класс для именованого объекта данных
    /// </summary>
    public class DataNamedObject : DataObject
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="aOwner">Ссылка на владельца объекта данных</param>
        public DataNamedObject(object aOwner) : base(aOwner)
        {
        }

        protected string _Name;
        /// <summary>
        /// Имя объекта данных
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    SetState(ObjectState.Modified);
                }
            }
        }
    }

    /// <summary>
    /// Базовый класс для именованого объекта данных для использования имени в качестве свойства
    /// </summary>
    public class DataNamedPropertyObject : DataNamedObject
    {
        public DataNamedPropertyObject(object aOwner) : base(aOwner)
        {
        }

        /// <summary>
        /// Имя свойства объекта данных
        /// </summary>
        public string NameProperty
        {
            get { return Helper.GetPropertyName(_Name); }
        }
    }
}
