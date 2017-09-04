using dpas.Core.Data.Attributes;

namespace dpas.Core.Data.BaseClasses
{
    [DataClass(Name = "Базовый класс справочника", Description = "Базовый класс справочника")]
    public class BaseReference : DataObject
    {
        public BaseReference() : base(null)
        {
        }

        public BaseReference(object aOwner) : base(aOwner)
        {
        }

        [DataProperty(Name = "Код", Description = "Код элемента справочника")]
        public string Code { get; set; }

        [DataProperty(Name = "Имя", Description = "Наименование элемента справочника")]
        public string Name { get; set; }
    }
}
