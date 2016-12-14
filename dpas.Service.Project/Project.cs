using System;
using System.Linq;
using System.Collections.Generic;
using dpas.Core;
using dpas.Core.Data;
using dpas.Core.Data.Specialization;

namespace dpas.Service.Project
{
    public class Project : DataNamedPropertyObject, IProject
    {
        private IList<IProject> _ProjectDependencies;

        public Project(object aOwner) : base(aOwner)
        {
            this.Init(Guid.NewGuid().ToString(), "Новый проект");
        }
        public Project(object aOwner, string aName, string aDescription) : base(aOwner)
        {
            this.Init(aName, aDescription);
        }

        private void Init(string aName, string aDescription)
        {
            this.Name = aName;
            this.Description = aDescription;
            _ProjectDependencies = new List<IProject>();
        }

        /// <summary>
        /// Переименование проекта
        /// </summary>
        /// <param name="aName">Новое имя</param>
        /// <param name="aDescription">Новое описание</param>
        public void Rename(string aName, string aDescription)
        {
            this.Name = aName;
            this.Description = aDescription;
            SetState(ObjectState.Modified);
        }

        /// <summary>
        /// Поиск ссылки на проект
        /// </summary>
        /// <param name="aName">Ссылка на проект</param>
        /// <returns>Найденная ссылка на проект</returns>
        private IProject FindProjectDependency(IProject aProject)
        {
            if (aProject == null)
                throw new ArgumentNullException("Не задана ссылка на проект");
            if (string.IsNullOrEmpty(aProject.Name))
                throw new ArgumentNullException("Для ссылки на проект не указано имя");
            return FindProjectDependency(aProject.Name);
        }

        /// <summary>
        /// Поиск ссылки на проект по имени
        /// </summary>
        /// <param name="aName">Имя проекта</param>
        /// <returns>Найденная ссылка на проект</returns>
        private IProject FindProjectDependency(string aName)
        {
            return _ProjectDependencies.FirstOrDefault(f => f.Name == aName);
        }

        /// <summary>
        /// Добавление ссылки на проект
        /// </summary>
        /// <param name="aProject">Ссылка на проект</param>
        public void AddProjectDependency(IProject aProject)
        {
            var find = FindProjectDependency(aProject);
            if (find == null)
            {
                _ProjectDependencies.Add(aProject);
                SetState(ObjectState.Modified);
            }

        }

        /// <summary>
        /// Удаление ссылки на проект
        /// </summary>
        /// <param name="aProject">Ссылка на проект</param>
        public void DeleteProjectDependency(IProject aProject)
        {
            var find = FindProjectDependency(aProject);
            if (find == null)
            {
                _ProjectDependencies.Remove(find);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ProjectDependencies.Clear();
                _ProjectDependencies = null;
                this.Name = this.Description = null;
            }
            base.Dispose(disposing);
        }

        public string Description { get; internal set; }

        /// <summary>
        /// Список ссылок на другие проекты
        /// </summary>
        public IList<IProject> ProjectDependencies
        {
            get
            {
                return _ProjectDependencies;
            }
        }

    }
}
