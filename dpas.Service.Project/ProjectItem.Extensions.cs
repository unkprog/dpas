using System.Xml;

namespace dpas.Service.Project
{
    public static class ProjectItemExtensions
    {
        public static void ReadItems(this IProjectItems projectItem, XmlReader Reader)
        {
            while (Reader.Read() && Reader.NodeType != XmlNodeType.EndElement)
            {
                if (Reader.NodeType == XmlNodeType.Element && !Reader.IsEmptyElement)
                {
                         if (Reader.Name == "Items") ReadItems(projectItem, Reader);
                    else if (Reader.Name == "ProjectItem")
                    {
                        ProjectItem projectItemNew = new ProjectItem(projectItem);
                        projectItemNew.Read(Reader);
                        projectItemNew.ID = projectItem.Items.Count;
                        projectItem.Items.Add(projectItemNew);
                    }
                }
            }
        }

        public static void WriteItems(this IProjectItems projectItem, XmlWriter Writer)
        {
            Writer.WriteStartElement("Items");
            for (int i = 0, icount = projectItem.Items.Count; i < icount; i++)
                ((IProjectItem)projectItem.Items[i]).Write(Writer);
            Writer.WriteEndElement();
        }
    }
}
