using System;
using System.Collections.Generic;
using dpas.Core;

namespace dpas.Service.Project
{
    public class Project : Disposable, IProject
    {
        private IList<IProject> _ProjectDependencies;

        public Project()
        {
            this.Init(Guid.NewGuid().ToString());
        }
        public Project(string Name)
        {
            this.Init(Name);
        }


        private void Init(string name)
        {
            this.Name = name;
            _ProjectDependencies = new List<IProject>();
        }

        public bool Rename(string Name, string Description)
        {
            throw new NotImplementedException();
        }

        public void AddPojectDependency(IProject Project)
        {
            throw new NotImplementedException();
        }

        public void DeletePojectDependency(IProject Project)
        {
            throw new NotImplementedException();
        }

        public string Name { get; internal set; }
        public string Description { get; internal set; }

        public IList<IProject> ProjectDependencies
        {
            get
            {
                return _ProjectDependencies;
            }
        }

    }
}
