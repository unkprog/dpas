using System.Collections.Generic;
using System.Xml;
using dpas.Core.Data.Specialization;
using dpas.Core.Extensions;

namespace dpas.Service.Project
{
    public partial class ProjectItem : DataNamedPropertyObject, IProjectItem
    {
        public ProjectItem(object aOwner) : base(aOwner)
        {
            Items = new List<IProjectItem>();
        }
        public int Index { get; internal set; }


        public string Description { get; internal set; }

        public IList<IProjectItem> Items { get; internal set; }

        public string Path { get; internal set; }

        public int Type    { get; internal set; }

        #region IReaderXml

        protected virtual void ReadProperties(XmlReader aReader)
        {
            Name = aReader.GetAttribute("Name");
            Path = aReader.GetAttribute("Path");
            Description = aReader.GetAttribute("Description");
            Type = aReader.GetInt32("Type");
        }
        public void Read(XmlReader aReader)
        {
            ReadProperties(aReader);
          
            while (aReader.Read() && aReader.NodeType != XmlNodeType.EndElement)
            {
                if (aReader.NodeType == XmlNodeType.Element && !aReader.IsEmptyElement)
                {
                    if (aReader.Name == "Items")
                        ProjectItemExtensions.ReadItems(this, aReader);
                }
            }
        }
        #endregion


        internal void SetupParams()
        {
            Path = Owner is IProject ? string.Concat(((IProject)Owner).Name, '/', Name)
                                    : Owner is IProjectItem ? string.Concat(((IProjectItem)Owner).Path, '/', Name)
                                                            : Name;
        }
        #region IWriterXml

        protected virtual void WriteProperties(XmlWriter aWriter)
        {
            aWriter.WriteAttributeString("Name", Name);
            aWriter.WriteAttributeString("Path", Path);
            aWriter.WriteAttributeString("Description", Description);
            aWriter.WriteAttributeString("Type", Type.ToString());
        }
        public void Write(XmlWriter aWriter)
        {
            aWriter.WriteStartElement("ProjectItem");

            WriteProperties(aWriter);
            ProjectItemExtensions.WriteItems(this, aWriter);

            aWriter.WriteEndElement();
        }
        #endregion
    }

    
}
