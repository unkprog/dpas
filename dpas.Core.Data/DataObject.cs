using System;

namespace dpas.Core.Data
{
    /// Интерфейс класса для объекта данных
    public interface IDataObject
    {
        object Owner { get; }

        ObjectState State { get; }
        bool IsStateNormal { get; }
        bool IsStateDeleted { get; }

        void SetState(ObjectState aState);
        event EventHandler StateChange;
    }

    /// <summary>
    /// Базовый класс для объекта данных
    /// </summary>
    public class DataObject : Disposable, IDataObject
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="aOwner">Ссылка на владельца объекта данных</param>
        public DataObject(object aOwner)
        {
            Owner = aOwner;
        }

        /// <summary>
        /// Состояние объекта
        /// </summary>
        public ObjectState State { get; private set; } = ObjectState.Created;

        public bool IsStateNormal {  get { return State == ObjectState.Normal; } }
        public bool IsStateDeleted { get { return State == ObjectState.Deleted; } }

        private event EventHandler _StateChange;
        /// <summary>
        /// Обработчик события изменения состояния.
        /// </summary>
        public event EventHandler StateChange { add { _StateChange += value; } remove { _StateChange -= value; } }

        /// <summary>
        /// Ссылка на владельца объекта данных
        /// </summary>
        public object Owner { get; protected set; }


        
        /// <summary>
        /// Изменение состояния объекта
        /// </summary>
        /// <param name="aState">Новое состояние</param>
        public void SetState(ObjectState aState)
        {
            State = aState;
            _StateChange?.Invoke(this, EventArgs.Empty);
        }


        protected override void Dispose(bool aDisposing)
        {
            if (aDisposing)
            {
                _StateChange = null;
                Owner = null;
            }
            base.Dispose(aDisposing);
        }
    }
}
