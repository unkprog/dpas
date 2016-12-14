using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dpas.Core;
using dpas.Core.Data;

namespace dpas.Service.Project
{
    public class ProjectManager : DataObject, IProjectManager
    {
        List<IProject> _Projects;


        public ProjectManager(object aOwner) : base(aOwner)
        {
            _Projects = new List<IProject>();
        }


        /// <summary>
        /// Список проектов
        /// </summary>
        public IList<IProject> Projects
        {
            get
            {
                return _Projects;
            }
        }

        /// <summary>
        /// Создание нового проекта
        /// </summary>
        /// <param name="aName">Имя проекта</param>
        /// <param name="aDecription">Описание проекта</param>
        /// <returns>Ссылка на проект</returns>
        public IProject Create(string aName, string aDecription)
        {
            IProject result = new Project(this, aName, aDecription);
            var find = FindProject(result);
            if(find == null)
            {
                _Projects.Add(result);
                SetState(ObjectState.Modified);
            }
            return result;
        }

        /// <summary>
        /// Поиск проекта
        /// </summary>
        /// <param name="aName">Ссылка на проект</param>
        /// <returns>Найденная ссылка на проект</returns>
        private IProject FindProject(IProject aProject)
        {
            if (aProject == null)
                throw new ArgumentNullException("Не задана ссылка на проект");
            if (string.IsNullOrEmpty(aProject.Name))
                throw new ArgumentNullException("Для ссылки на проект не указано имя");
            return FindProject(aProject.Name);
        }

        /// <summary>
        /// Поиск проекта по имени
        /// </summary>
        /// <param name="aName">Имя проекта</param>
        /// <returns>Найденная ссылка на проект</returns>
        private IProject FindProject(string aName)
        {
            return _Projects.FirstOrDefault(f => f.Name == aName);
        }

        public void Delete(IProject aProject)
        {
            var find = FindProject(aProject);
            if (find == null)
            {
                _Projects.Remove(find);
                SetState(ObjectState.Modified);
            }
            
        }

        public bool Save(IProject Project)
        {
            if (!IsStateNormal)
            {
                throw new NotImplementedException();
                return true;
            }
            return false;
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _Projects.Clear();
                _Projects = null;
            }
            base.Dispose(disposing);
        }
    }
}
