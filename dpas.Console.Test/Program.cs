using System.IO;
using System.Xml;
using dpas.Service.Project;

namespace dpas.Console.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ProjectManager mng = new ProjectManager(null);
            mng.Create("Проект 1", "Проект 1");
            mng.Create("Проект 2", "Проект 2");

            using (TextWriter textWriter = File.CreateText("projects.xml"))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, new XmlWriterSettings() { Indent = true, IndentChars = "\t", NewLineChars = System.Environment.NewLine }))
                {
                    xmlWriter.WriteStartDocument();
                    mng.Write(xmlWriter);
                    xmlWriter.WriteEndDocument();
                }
            }

            using (XmlReader xmlReader = XmlReader.Create("projects.xml"))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Projects")
                    {
                        mng.Read(xmlReader);
                    }
                }
            }
        }
    }
}
