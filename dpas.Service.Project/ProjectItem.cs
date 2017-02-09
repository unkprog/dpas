using System.Collections.Generic;
using System.Xml;
using dpas.Core.Data.Specialization;

namespace dpas.Service.Project
{
    public class ProjectItem : DataNamedPropertyObject, IProjectItem
    {
        public ProjectItem(object aOwner) : base(aOwner)
        {
            Items = new List<IProjectItem>();
        }
        public int ID { get; internal set; }

        public IList<IProjectItem> Items { get; internal set; }

        public string Path { get; internal set; }

        public int Type    { get; internal set; }

        #region IReaderXml
        public void Read(XmlReader Reader)
        {
            Name = Reader.GetAttribute("Name");
            Path = Reader.GetAttribute("Path");

            while (Reader.Read() && Reader.NodeType != XmlNodeType.EndElement)
            {
                if (Reader.NodeType == XmlNodeType.Element && Reader.Name == "Items")
                    ProjectItemExtensions.ReadItems(this, Reader);
            }
        }
        #endregion

        #region IWriterXml
        public void Write(XmlWriter Writer)
        {
            Writer.WriteStartElement("ProjectItem");

            Writer.WriteAttributeString("Name", Name);
            Writer.WriteAttributeString("Path", Path);
            Writer.WriteAttributeString("Type", Type.ToString());

            ProjectItemExtensions.WriteItems(this, Writer);

            Writer.WriteEndElement();
        }
        #endregion
    }

    
}
