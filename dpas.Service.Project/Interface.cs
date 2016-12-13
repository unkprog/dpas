using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dpas.Service.Project
{
    public interface IProject
    {
        string Name { get; }

        string Description { get; }

        bool Rename(string Name, string Description);

        IList<IProject> ProjectDependencies { get; }

        void AddPojectDependency(IProject Project);
        void DeletePojectDependency(IProject Project);
    }


    public interface IProjectManager
    {
        IList<IProject> Projects { get; }

        IProject Create(string Name, string Decription);
        void Delete(IProject Project);
        bool Save(IProject Project);
    }

}
