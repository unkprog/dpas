using System.Collections.Generic;
using System.IO;

namespace dpas.Service.Project
{
    public partial class ProjectManager
    {

        /// <summary>
        /// Поиск элемента проекта
        /// </summary>
        /// <param name="aName">Ссылка на элемент проекта</param>
        /// <returns>Найденная ссылка на элемент проекта</returns>
        private IProjectItems FindProjectItem(IProject aProject, IProjectItems aItem)
        {
            if (aItem == null)
                throw new Project.Exception(Project.Exception.ArgumentNull);
            return FindProjectItem(aProject, aItem.Name);
        }

        /// <summary>
        /// Поиск элемента проекта
        /// </summary>
        /// <param name="aName">Ссылка на элемент проекта</param>
        /// <returns>Найденная ссылка на элемент проекта</returns>
        private IProjectItems FindProjectItem(IProject aProject, string aName)
        {
            if (aProject == null)
                throw new Project.Exception(Project.Exception.ArgumentNull);
            if (string.IsNullOrEmpty(aName))
                throw new Project.Exception(Project.Exception.EmptyName);
            return FindProjectItemByName(aProject.Items, aName);
        }

        /// <summary>
        /// Поиск элемента проекта по имени
        /// </summary>
        /// <param name="aName">Имя элемента проекта</param>
        /// <returns>Найденная ссылка на элемент проекта</returns>
        private IProjectItems FindProjectItemByName(IList<IProjectItems> aItems, string aName, bool aFindInChilds = true)
        {
            IProjectItems result = null;
            for (int i = 0, icount = aItems.Count; result == null && i < icount; i++)
            {
                if (aItems[i].Name == aName)
                    result = aItems[i];
                else
                    if (aFindInChilds)
                    result = FindProjectItemByName(aItems[i].Items, aName);
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
            IProjectItems parent = FindProjectItem(aProject, aParent);
            if (parent == null)
            {
                if (aProject.Name != aParent)
                    throw new Project.Exception(Project.Exception.NotFound, aParent);
                else
                    parent = aProject;
            }

            IProjectItems find = FindProjectItemByName(parent.Items, aName, false);
            if (find != null)
                throw new Project.Exception(Project.Exception.ItemAlreadyExists, aName);

            result = new ProjectItem(parent) { Type = aType, Name = aName };
            result.SetupParams();

            string path = string.Concat(pathProjects, "/", result.Path);

            if ((aType == ProjectItem.Reference || aType == ProjectItem.Data) && !Directory.Exists(path))
                Directory.CreateDirectory(path);
            parent.Items.Add(result);
            SaveProject(aProject);
            return result;
        }

    }
}
