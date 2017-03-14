using System.Xml;

namespace dpas.Service.Project
{
    public static class ProjectItemExtensions
    {
        public static void ReadItems(this IProjectItem projectItem, XmlReader Reader)
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
                        projectItemNew.Index = projectItem.Items.Count;
                        projectItem.Items.Add(projectItemNew);
                    }
                }
            }
        }

        public static void WriteItems(this IProjectItem projectItem, XmlWriter Writer)
        {
            Writer.WriteStartElement("Items");
            for (int i = 0, icount = projectItem.Items.Count; i < icount; i++)
                ((IProjectItem)projectItem.Items[i]).Write(Writer);
            Writer.WriteEndElement();
        }


        public static string GetStringType(this IProjectItem projectItem)
        {
            switch (projectItem.Type)
            {
                case ProjectItem.FieldInt: return "int";
                case ProjectItem.FieldDouble: return "double";
                case ProjectItem.FieldBool: return "bool";
                case ProjectItem.FieldDate: return "DateTime";
                case ProjectItem.FieldClass: return ((IProjectItemField)projectItem).TypeClass;
                default:
                    return "string";
            }
        }
    }
}
