using System;
using System.Collections.Generic;
using dpas.Net.Http.Mvc;
using dpas.Service.Project;
using System.Text;

namespace dpas.Net.Http.Mvc.Api.Prj
{
    public class Manager : IController
    {
        public virtual void Exec(ControllerContext context)
        {
            if (context.ControllerInfo.Action == "/list")
                List(context);
            else if (context.ControllerInfo.Action == "/create")
                Create(context);
            else if (context.ControllerInfo.Action == "/delete")
                Delete(context);
            else if (context.ControllerInfo.Action == "/rename")
                Rename(context);
            else
            {
                context.Response.ContentType = "application/json";// application / json; charset = UTF - 8
                                                                  //context.Response.Headers.Add("charset", "UTF-8");
                context.Response.Write(@"{""result"": true}");
            }
        }

        /// <summary>
        /// Получение списка проектов
        /// </summary>
        private void List(ControllerContext context)
        {
            ProjectManager.Manager.Read();

            context.Response.ContentType = "application/json";
            StringBuilder result = new StringBuilder("[");
            IProject project;
            IList<IProject> projects = ProjectManager.Manager.Projects;
            for (int i = 0, icount = projects.Count; i < icount; i++)
            {
                project = projects[i];
                if(i > 0)
                    result.Append(",");
                result.Append("{ \"Code\":");
                result.Append(string.Concat("\"", project.Code, "\""));
                result.Append(", \"Name\":");
                result.Append(string.Concat("\"", project.Name, "\""));
                result.Append(",\"Description\":");
                result.Append(string.Concat("\"", project.Description, "\""));
                result.Append("}");
            }
            result.Append("]");
            context.Response.Write(result.ToString());
        }
        /// <summary>
        /// Создание нового проекта
        /// </summary>
        private void Create(ControllerContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(@"{""result"": true}");
        }
        /// <summary>
        /// Удаление проекта
        /// </summary>
        private void Delete(ControllerContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(@"{""result"": true}");
        }

        /// <summary>
        /// Переименование проекта
        /// </summary>
        private void Rename(ControllerContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(@"{""result"": true}");
        }
    }
}
