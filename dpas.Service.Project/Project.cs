using System;
using System.Linq;
using System.Collections.Generic;
using dpas.Core.IO;
using dpas.Core.Data;
using dpas.Core.Data.Specialization;
using System.Xml;
using System.IO;

namespace dpas.Service.Project
{
    public class Project : DataNamedPropertyObject, IProject
    {
        public Project(object aOwner) : base(aOwner)
        {
            Init(Guid.NewGuid().ToString(), "Новый проект");
        }
        public Project(object aOwner, string aName, string aDescription) : base(aOwner)
        {
            Init(aName, aDescription);
        }

        private void Init(string aName, string aDescription)
        {
            Name = aName;
            Description = aDescription;
            _ProjectDependencies = new List<IProject>();
        }

        #region IProject


        public string Description { get; internal set; }

        private IList<IProject> _ProjectDependencies;
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
        /// <summary>
        /// Переименование проекта
        /// </summary>
        /// <param name="aName">Новое имя</param>
        /// <param name="aDescription">Новое описание</param>
        public void Rename(string aName, string aDescription)
        {
            Name = aName;
            Description = aDescription;
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
        #endregion

        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ProjectDependencies.Clear();
                _ProjectDependencies = null;
                Name = Description = null;
            }
            base.Dispose(disposing);
        }
        #endregion

        #region IWriterXml
        public void Write(XmlWriter Writer)
        {
            Writer.WriteStartElement("Project");
            Writer.WriteAttributeString("Name", Name);
            Writer.WriteAttributeString("Description", Description);

            Writer.WriteStartElement("ProjectDependencies");
            for (int i = 0, icount = _ProjectDependencies.Count; i < icount; i++)
            {
                Writer.WriteStartElement("Project");
                Writer.WriteAttributeString("Name", _ProjectDependencies[i].Name);
                Writer.WriteAttributeString("Description", _ProjectDependencies[i].Description);
                Writer.WriteEndElement();
            }
            Writer.WriteEndElement();

            Writer.WriteEndElement();
        }

        private void CheckProjectDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        public void Save(string path)
        {
            string pathProject = path;
            if (pathProject == null) pathProject = string.Empty;

            CheckProjectDirectory(pathProject);

            pathProject = string.Concat(pathProject, @"\\", Name, ".dpj");
            using (TextWriter textWriter = File.CreateText(pathProject))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, new XmlWriterSettings() { Indent = true, IndentChars = "\t", NewLineChars = System.Environment.NewLine }))
                {
                    xmlWriter.WriteStartDocument();
                    Write(xmlWriter);
                    xmlWriter.WriteEndDocument();
                }
            }
        }
        #endregion

    }
}
