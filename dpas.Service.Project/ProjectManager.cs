using System;
using System.Collections.Generic;
using System.Linq;
using dpas.Core.IO;
using dpas.Core.Data;
using System.Xml;
using System.IO;

namespace dpas.Service.Project
{
    public partial class ProjectManager : DataObject, IProjectManager, IReaderXml, IWriterXml
    {
        public readonly static ProjectManager Manager = new ProjectManager(null);

        List<IProject> _Projects;
        private string pathProjects = "Projects";
        internal ProjectManager(object aOwner) : base(aOwner)
        {
            _Projects = new List<IProject>();
            Read();
        }

        #region IProjectManager
        /// <summary>
        /// Список проектов
        /// </summary>
        public IList<IProject> Projects
        {
            get
            {
                //if(_Projects == null)
                //{
                //    _Projects = new List<IProject>();
                //    Read();
                //}
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
            Save(result, true);
            return result;
        }

        /// <summary>
        /// Переименование проекта
        /// </summary>
        /// <param name="OldName">Старое имя</param>
        /// <param name="Name">Имя</param>
        /// <param name="Decription">Описание</param>
        /// <returns>Переименованный проект</returns>
        public IProject Rename(string OldName, string Name, string Decription)
        {
            IProject result = FindProjectByName(OldName);
            if (result == null)
                throw new Project.Exception(Project.Exception.NotFound, OldName);
            var proj = result as Project;
            proj.Name = Name;
            proj.Description = Decription;
            SetState(ObjectState.Modified);
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
                throw new Project.Exception(Project.Exception.ArgumentNull);
            if (string.IsNullOrEmpty(aProject.Name))
                throw new Project.Exception(Project.Exception.EmptyName);

            return FindProjectByName(aProject.Name);
        }

        /// <summary>
        /// Поиск проекта по имени
        /// </summary>
        /// <param name="aName">Имя проекта</param>
        /// <returns>Найденная ссылка на проект</returns>
        public IProject FindProjectByName(string aName)
        {
            return _Projects.FirstOrDefault(f => f.Name == aName);
        }

        /// <summary>
        /// Поиск проекта по коду
        /// </summary>
        /// <param name="aName">Код проекта</param>
        /// <returns>Найденная ссылка на проект</returns>
        public IProject FindProjectByCode(string aCode)
        {
            return _Projects.FirstOrDefault(f => f.Code == aCode);
        }
        /// <summary>
        /// Удаление проекта
        /// </summary>
        /// <param name="aProject">Удаляемый проект</param>
        public void Delete(IProject aProject)
        {
            var find = FindProject(aProject);
            if (find != null)
            {
                _Projects.Remove(find);
                SetState(ObjectState.Modified);
                CheckProjectsDirectory();
                string projectFile = string.Concat(pathProjects, @"\\", find.Name);
                Directory.Delete(projectFile, true);
            }
            
        }

        /// <summary>
        /// Сохранить проект
        /// </summary>
        /// <param name="aProject">Сохраняемый проект</param>
        public void Save(IProject aProject)
        {
            Save(aProject, false);
        }

        public IProject AddProject(IProject aProject, bool isRead)
        {
            IProject find = FindProject(aProject);
            if (find == null)
            {
                _Projects.Add(aProject);
                if (!isRead)
                    SetState(ObjectState.Modified);
            }
            return find;
        }

        public void Save(IProject aProject, bool aNew)
        {
            var find = AddProject(aProject, false);
            if (find != null && aNew)
                throw new Project.Exception(Project.Exception.AlreadyExists, find.Name);
            SaveProject(aProject);
        }

        private void CheckProjectsDirectory()
        {
            if (!Directory.Exists(pathProjects))
                Directory.CreateDirectory(pathProjects);
        }

        public void SaveProject(IProject aProject)
        {
            CheckProjectsDirectory();
            string projectFile = string.Concat(pathProjects, @"\\", aProject.Name);
            ((Project)aProject).Save(projectFile);
        }

        public void ReadProject(IProject aProject)
        {
            CheckProjectsDirectory();
            string projectFile = string.Concat(pathProjects, @"\\", aProject.Name);
            ((Project)aProject).Read(projectFile);
        }

        public void Save()
        {
            CheckProjectsDirectory();

            for (int i = 0, icount = _Projects.Count; i < icount; i++)
                SaveProject(_Projects[i]);

            string projectsFile = string.Concat(pathProjects, @"\\projects.dps");
            using (TextWriter textWriter = File.CreateText(projectsFile))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, new XmlWriterSettings() { Indent = true, IndentChars = "\t", NewLineChars = Environment.NewLine }))
                {
                    xmlWriter.WriteStartDocument();
                    Write(xmlWriter);
                    xmlWriter.WriteEndDocument();
                }
            }
        }

        public void Read()
        {
            CheckProjectsDirectory();

            string projectsFile = string.Concat(pathProjects, @"\\projects.dps");
            if (File.Exists(projectsFile))
            {
                using (XmlReader xmlReader = XmlReader.Create(projectsFile))
                {
                    while (xmlReader.Read())
                    {
                        if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Projects")
                        {
                            Read(xmlReader);
                        }
                    }
                }
            }
            SetState(ObjectState.Normal);
        }

        
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _Projects.Clear();
                _Projects = null;
            }
            base.Dispose(disposing);
        }

        #region IReaderXml
        public void Read(XmlReader Reader)
        {
            _Projects.Clear();
            while (Reader.Read() && Reader.NodeType != XmlNodeType.EndElement)
            {
                if (Reader.NodeType == XmlNodeType.Element && Reader.Name == "Project")
                {
                    AddProject(new Project(this) { Code = Reader.GetAttribute("Code"), Name = Reader.GetAttribute("Name"), Description = Reader.GetAttribute("Description") }, false);
                }
            }
        
        }
        #endregion

        #region IWriterXml
        public void Write(XmlWriter Writer)
        {
            Writer.WriteStartElement("Projects");
            for (int i = 0, icount = _Projects.Count; i < icount; i++)
            {
                Writer.WriteStartElement("Project");
                Writer.WriteAttributeString("Code", _Projects[i].Code);
                Writer.WriteAttributeString("Name", _Projects[i].Name);
                Writer.WriteAttributeString("Description", _Projects[i].Description);
                Writer.WriteEndElement();
            }
            Writer.WriteEndElement();
        }
        #endregion
    }
}
