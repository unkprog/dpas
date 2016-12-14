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
        bool Save(IProject Project);
    }

}
