﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dpas.Core.IO;
using dpas.Core.Data;
using System.Xml;

namespace dpas.Service.Project
{
    public class ProjectManager : DataObject, IProjectManager, IReaderXml, IWriterXml
    {
        List<IProject> _Projects;

        public ProjectManager(object aOwner) : base(aOwner)
        {
            _Projects = new List<IProject>();
        }

        #region IProjectManager
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
            Save(result);
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

        /// <summary>
        /// Удаление проекта
        /// </summary>
        /// <param name="aProject">Удаляемый проект</param>
        public void Delete(IProject aProject)
        {
            var find = FindProject(aProject);
            if (find == null)
            {
                _Projects.Remove(find);
                SetState(ObjectState.Modified);
            }
            
        }

        /// <summary>
        /// Сохранить проект
        /// </summary>
        /// <param name="aProject">Сохраняемый проект</param>
        public void Save(IProject aProject)
        {
            var find = FindProject(aProject);
            if (find == null)
            {
                _Projects.Add(aProject);
                SetState(ObjectState.Modified);
            }
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
                    Save(Create(Reader.GetAttribute("Name"), Reader.GetAttribute("Description")));
                }
            }
        
        }
        #endregion

        #region IWriterXml
        public void Write(XmlWriter Writer)
        {
            //Writer.WriteString(Environment.NewLine);
            Writer.WriteStartElement("Projects");
            for (int i = 0, icount = _Projects.Count; i < icount; i++)
            {
                //Writer.WriteString(string.Concat(Environment.NewLine, "  "));
                Writer.WriteStartElement("Project");
                Writer.WriteAttributeString("Name", _Projects[i].Name);
                Writer.WriteAttributeString("Description", _Projects[i].Description);
                Writer.WriteEndElement();
            }
            //Writer.WriteString(Environment.NewLine);
            Writer.WriteEndElement();
        }
        #endregion
    }
}
