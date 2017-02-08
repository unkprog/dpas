using System.Collections.Generic;
using dpas.Core.IO;

namespace dpas.Service.Project
{
    
    public interface IRead
    {
        void Read();
    }

    public interface IProject : IReaderXml, IWriterXml
    {
        /// <summary>
        /// Код проекта
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Имя проекта
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Описание проекта
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Переименование проекта
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <param name="Description">Описание</param>
        void Rename(string Name, string Description);

        /// <summary>
        /// Список проектов, от которых зависит данный проект
        /// </summary>
        IList<IProject> ProjectDependencies { get; }

        /// <summary>
        /// Добавление зависимости от проекта
        /// </summary>
        /// <param name="Project">Проект</param>
        void AddProjectDependency(IProject Project);

        /// <summary>
        /// Удаление зависимости
        /// </summary>
        /// <param name="Project"></param>
        void DeleteProjectDependency(IProject Project);
    }


    public interface IProjectManager
    {
        /// <summary>
        /// Список проектов
        /// </summary>
        IList<IProject> Projects { get; }

        /// <summary>
        /// Создание проекта
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <param name="Decription">Описание</param>
        /// <returns>Новый проект</returns>
        IProject Create(string Name, string Decription);

        /// <summary>
        /// Переименование проекта
        /// </summary>
        /// <param name="OldName">Старое имя</param>
        /// <param name="Name">Имя</param>
        /// <param name="Decription">Описание</param>
        /// <returns>Переименованный проект</returns>
        IProject Rename(string OldName, string Name, string Decription);

        /// <summary>
        /// Удаление проекта
        /// </summary>
        /// <param name="Project">Проект</param>
        void Delete(IProject Project);

        /// <summary>
        /// Сохранение проекта
        /// </summary>
        /// <param name="Project">Проект</param>
        void Save(IProject Project);

        /// <summary>
        /// Поиск проекта поимени
        /// </summary>
        /// <param name="aName">Имя проекта</param>
        /// <returns>Найденный проект</returns>
        IProject FindProject(string aName);
    }

}
