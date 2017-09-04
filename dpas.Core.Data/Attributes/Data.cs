using System;

namespace dpas.Core.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class BaseAttribute : Attribute
    {
        // Имя
        public string Name { get; set; }
        // Описание
        public string Description { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DataClassAttribute : BaseAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DataPropertyAttribute : BaseAttribute
    {
    }

}
