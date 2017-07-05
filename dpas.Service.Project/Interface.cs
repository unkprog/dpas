using System.Collections.Generic;
using dpas.Core.IO;

namespace dpas.Service.Project
{
    public interface IProjectItem : IReaderXml, IWriterXml
    {
        /// <summary>
        /// Абстрактный
        /// </summary>
       bool IsAbstract { get; }

        /// <summary>
        /// Имя
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Путь в файловой директории
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Список элементов проекта
        /// </summary>
        IList<IProjectItem> Items { get; }
   
        /// <summary>
        /// Индекс
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Тип
        /// </summary>
        int Type { get; }

        /// <summary>
        /// Описание
        /// </summary>
        string Description { get; }
    }

    public interface IProjectItemField : IProjectItem
    {
        /// <summary>
        /// Наименование класса
        /// </summary>
        string TypeClass { get; }
    }

    public interface IProject : IProjectItem, IReaderXml, IWriterXml
    {
        /// <summary>
        /// Код проекта
        /// </summary>
        string Code { get; }

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
        IProject FindProjectByName(string aName);
    }

}
