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
            IProject prj = mng.Create("Проект 1", "Проект 1");
            mng.Create("Проект 2", "Проект 2").AddProjectDependency(prj);
           
            mng.Save();

            //using (XmlReader xmlReader = XmlReader.Create("projects.xml"))
            //{
            //    while (xmlReader.Read())
            //    {
            //        if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Projects")
            //        {
            //            mng.Read(xmlReader);
            //        }
            //    }
            //}
        }
    }
}
