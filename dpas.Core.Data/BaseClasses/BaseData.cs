
using dpas.Core.Data.Attributes;
using System;

namespace dpas.Core.Data.BaseClasses
{
    [DataClass(Name = "Базовый класс данных", Description = "Базовый класс данных")]
    public class BaseData : DataObject
    {
        public BaseData() : base(null)
        {
        }

        public BaseData(object aOwner) : base(aOwner)
        {
        }

        [DataProperty(Name = "Дата", Description = "Дата значения")]
        public DateTime Date { get; set; }

        [DataProperty(Name = "Значение", Description = "Значение")]
        public double Value { get; set; }

    }
}
