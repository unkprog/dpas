using System.Collections.Generic;
using dpas.Core.IO;

namespace dpas.Service.Project
{
    
    public interface IRead
    {
        void Read();
    }

    public interface IProject : /*IReaderXml,*/ IWriterXml
    {
        string Name { get; }

        string Description { get; }

        void Rename(string Name, string Description);

        IList<IProject> ProjectDependencies { get; }

        void AddProjectDependency(IProject Project);
        void DeleteProjectDependency(IProject Project);
    }


    public interface IProjectManager
    {
        IList<IProject> Projects { get; }

        IProject Create(string Name, string Decription);
        void Delete(IProject Project);
        void Save(IProject Project);
    }

}
