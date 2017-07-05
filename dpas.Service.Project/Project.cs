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
    public partial class Project : DataNamedPropertyObject, IProject
    {
        public Project(object aOwner) : base(aOwner)
        {
            string guid = Guid.NewGuid().ToString();
            Init(guid.Replace("-", string.Empty), guid, "Новый проект");
        }
        public Project(object aOwner, string aName, string aDescription) : base(aOwner)
        {
            Init(Guid.NewGuid().ToString().Replace("-", string.Empty), aName, aDescription);
        }

        private void Init(string aCode, string aName, string aDescription)
        {
            Code = aCode;
            Name = aName;
            Description = aDescription;
            _ProjectDependencies = new List<IProject>();
            _Items = new List<IProjectItem>();
        }

        #region IProject

        public bool IsAbstract { get { return false; } }
        public string Path { get { return Name; } }

        public string Code { get; internal set; }
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
                throw new Exception(Exception.ArgumentNull);
            if (string.IsNullOrEmpty(aProject.Name))
                throw new Exception(Exception.EmptyName);
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

        private IList<IProjectItem> _Items;
        /// <summary>
        /// Список элементов проекта
        /// </summary>
        public IList<IProjectItem> Items
        {
            get
            {
                return _Items;
            }
        }

        public int Index { get; internal set; }

        public int Type { get; internal set; }
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

        private void CheckProjectDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        #region IReaderXml
        public void Read(XmlReader Reader)
        {
            Code = Reader.GetAttribute("Code");
            Name = Reader.GetAttribute("Name");
            Description = Reader.GetAttribute("Description");

            while (Reader.Read() && Reader.NodeType != XmlNodeType.EndElement)
            {
                if (Reader.NodeType == XmlNodeType.Element && !Reader.IsEmptyElement)
                {
                         if (Reader.Name == "ProjectDependencies") ReadProjectDependencies(Reader);
                    else if (Reader.Name == "Items")               ProjectItemExtensions.ReadItems(this, Reader);
                }
            }
        }

        public void ReadProjectDependencies(XmlReader Reader)
        {
            while (Reader.Read() && Reader.NodeType != XmlNodeType.EndElement)
            {
                if (Reader.NodeType == XmlNodeType.Element && !Reader.IsEmptyElement)
                {
                    if (Reader.Name == "Project")
                    {
                        IProject project = new Project(ProjectManager.Manager);
                        project.Read(Reader);
                        _ProjectDependencies.Add(project);
                    }
                }
            }
        }

        public void ReadProjectItems(XmlReader Reader)
        {
            while (Reader.Read() && Reader.NodeType != XmlNodeType.EndElement)
            {
                if (Reader.Name == "ProjectItem")
                {
                    IProjectItem projectItem = new ProjectItem(ProjectManager.Manager);
                    projectItem.Read(Reader);
                    Items.Add(projectItem);
                }
            }
        }

        public void Read(string path)
        {
            string pathProject = path;
            pathProject = string.Concat(pathProject, @"\\", Name, ".dpj");
            if (File.Exists(pathProject))
            {
                using (XmlReader xmlReader = XmlReader.Create(pathProject))
                {
                    while (xmlReader.Read())
                    {
                        if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Project")
                            Read(xmlReader);
                    }
                }
            }
        }
        #endregion

        #region IWriterXml
        public void Write(XmlWriter Writer)
        {
            Writer.WriteStartElement("Project");
            Writer.WriteAttributeString("Code", Code);
            Writer.WriteAttributeString("Name", Name);
            Writer.WriteAttributeString("Description", Description);

            Writer.WriteStartElement("ProjectDependencies");
            for (int i = 0, icount = _ProjectDependencies.Count; i < icount; i++)
            {
                Writer.WriteStartElement("Project");
                Writer.WriteAttributeString("Code", _ProjectDependencies[i].Code);
                Writer.WriteAttributeString("Name", _ProjectDependencies[i].Name);
                Writer.WriteAttributeString("Description", _ProjectDependencies[i].Description);
                Writer.WriteEndElement();
            }
            Writer.WriteEndElement();

            ProjectItemExtensions.WriteItems(this, Writer);

            Writer.WriteEndElement();
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
