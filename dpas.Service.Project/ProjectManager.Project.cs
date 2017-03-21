using System.Collections.Generic;
using System.IO;
using System.Text;
using dpas.Core.Extensions;

namespace dpas.Service.Project
{
    public partial class ProjectManager
    {

        /// <summary>
        /// Поиск элемента проекта
        /// </summary>
        /// <param name="aName">Ссылка на элемент проекта</param>
        /// <returns>Найденная ссылка на элемент проекта</returns>
        private IProjectItem FindProjectItem(IProject aProject, IProjectItem aItem)
        {
            if (aItem == null)
                throw new Project.Exception(Project.Exception.ArgumentNull);
            return FindProjectItem(aProject, aItem.Name);
        }

        /// <summary>
        /// Поиск элемента проекта
        /// </summary>
        /// <param name="aPath">Ссылка на элемент проекта</param>
        /// <returns>Найденная ссылка на элемент проекта</returns>
        public IProjectItem FindProjectItem(IProject aProject, string aPath)
        {
            if (aProject == null)
                throw new Project.Exception(Project.Exception.ArgumentNull);
            if (string.IsNullOrEmpty(aPath))
                throw new Project.Exception(Project.Exception.EmptyName);
            return FindProjectItemByPath(aProject.Items, aPath);
        }

        /// <summary>
        /// Поиск элемента проекта по имени
        /// </summary>
        /// <param name="aPath">Имя элемента проекта</param>
        /// <returns>Найденная ссылка на элемент проекта</returns>
        private IProjectItem FindProjectItemByPath(IList<IProjectItem> aItems, string aPath, bool aFindInChilds = true)
        {
            IProjectItem result = null;
            for (int i = 0, icount = aItems.Count; result == null && i < icount; i++)
            {
                if (aItems[i].Path == aPath)
                    result = aItems[i];
                else
                    if (aFindInChilds)
                    result = FindProjectItemByPath(aItems[i].Items, aPath);
            }
            return result;
        }

        /// <summary>
        /// Создание нового элемента проекта
        /// </summary>
        /// <param name="aName">Имя элемента проекта</param>
        /// <param name="aDecription">Описание элемента проекта</param>
        /// <returns>Ссылка на проект</returns>
        public IProjectItem ProjectAddItem(IProject aProject, string aParent, int aType, string aName, string aDecription)
        {
            ProjectItem result = null;
            if (string.IsNullOrEmpty(aName))
                throw new Project.Exception(Project.Exception.ItemEmptyName);
            IProjectItem parent = FindProjectItem(aProject, aParent);
            if (parent == null)
            {
                if (aProject.Name != aParent)
                    throw new Project.Exception(Project.Exception.NotFound, aParent);
                else
                    parent = aProject;
            }

            result = new ProjectItem(parent) { Type = aType, Name = aName, Description = aDecription };
            result.SetupParams();

            IProjectItem find = FindProjectItemByPath(parent.Items, result.Path, false);
            if (find != null)
                throw new Project.Exception(Project.Exception.ItemAlreadyExists, aName);

            string path = string.Concat(pathProjects, "/", result.Path);

            if (aType == ProjectItem.Reference || aType == ProjectItem.Data)
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            else if (aType == ProjectItem.ReferenceItem || aType == ProjectItem.DataItem)
            {
                if (!File.Exists(path))
                    SaveItemToFile(result);
            }

            parent.Items.Add(result);
            SaveProject(aProject);
            return result;
        }


        private void SaveItemToFile(IProjectItem aItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("namespace ");
            sb.AppendLine(aItem.Path.Substring(0, aItem.Path.Length - (string.Concat("/", aItem.Name)).Length).Replace('/', '.'));
            sb.AppendLine("{");
            sb.Append("    public class ");
            sb.AppendLine(aItem.Name);
            sb.AppendLine("    {");
            IProjectItem item;
            for (int i = 0, icount = aItem.Items.Count; i < icount; i++)
            {
                item = aItem.Items[i];
                sb.AppendLine(string.Concat("        public ", item.GetStringType(), " ", item.Name, " { get; set; }"));
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
            string file = string.Concat(pathProjects, "/", aItem.Path, ".cs");
            using (TextWriter textWriter = File.CreateText(file))
            {
                textWriter.Write(sb.ToString());
            }
        }

        public IProjectItem ProjectSaveItem(IProject aProject, string aPath, List<object> aFields)
        {
            IProjectItem result = FindProjectItem(aProject, aPath);
            if (result == null)
                throw new Project.Exception(Project.Exception.NotFound, aPath);

            result.Items.Clear();
            foreach (var fieldItem in aFields)
            {
                result.Items.Add(new ProjectItemField(result)
                {
                    Name = ((Dictionary<string, object>)fieldItem).GetString("Name"),
                    Description = ((Dictionary<string, object>)fieldItem).GetString("Description"),
                    Type = ((Dictionary<string, object>)fieldItem).GetInt32("Type"),
                    TypeClass = ((Dictionary<string, object>)fieldItem).GetString("TypeClass")
                });
            }
            SaveItemToFile(result);
            SaveProject(aProject);
            return result;
        }
    }        
}
