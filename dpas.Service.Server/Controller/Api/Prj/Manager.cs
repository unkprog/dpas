using System;
using System.Collections.Generic;
using dpas.Net.Http.Mvc;
using dpas.Service.Project;
using System.Text;

namespace dpas.Net.Http.Mvc.Api.Prj
{
    public class Manager : IController
    {
        public virtual void Exec(HttpContext context, ControllerInfo controllerInfo)
        {
            if (controllerInfo.Action == "/list")
                List(context, controllerInfo);
            else if (controllerInfo.Action == "/create")
                Create(context, controllerInfo);
            else if (controllerInfo.Action == "/delete")
                Delete(context, controllerInfo);
            else if (controllerInfo.Action == "/rename")
                Rename(context, controllerInfo);
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
        private void List(HttpContext context, ControllerInfo controllerInfo)
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
        private void Create(HttpContext context, ControllerInfo controllerInfo)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(@"{""result"": true}");
        }
        /// <summary>
        /// Удаление проекта
        /// </summary>
        private void Delete(HttpContext context, ControllerInfo controllerInfo)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(@"{""result"": true}");
        }

        /// <summary>
        /// Переименование проекта
        /// </summary>
        private void Rename(HttpContext context, ControllerInfo controllerInfo)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(@"{""result"": true}");
        }
    }
}
