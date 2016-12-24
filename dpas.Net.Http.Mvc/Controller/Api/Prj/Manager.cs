using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using dpas.Net.Http.Mvc;
using dpas.Service.Project;
using System.Text;

namespace dpas.Net.Http.Mvc.Controller.Api.Prj
{
    public class Manager : dpas.Net.Http.Mvc.IController
    {
        public virtual void Exec(HttpContext context, ControllerInfo controllerInfo, Dictionary<string, object> state)
        {
            if (controllerInfo.Action == "/list")
                List(context, controllerInfo, state);
            else if (controllerInfo.Action == "/create")
                Create(context, controllerInfo, state);
            else if (controllerInfo.Action == "/delete")
                Delete(context, controllerInfo, state);
            else if (controllerInfo.Action == "/rename")
                Rename(context, controllerInfo, state);
            else
            {
                context.Response.ContentType = "application/json";// application / json; charset = UTF - 8
                                                                  //context.Response.Headers.Add("charset", "UTF-8");
                context.Response.WriteAsync(@"{""result"": true}");
            }
        }

        /// <summary>
        /// Получение списка проектов
        /// </summary>
        private void List(HttpContext context, ControllerInfo controllerInfo, Dictionary<string, object> state)
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
            context.Response.WriteAsync(result.ToString());
        }
        /// <summary>
        /// Создание нового проекта
        /// </summary>
        private void Create(HttpContext context, ControllerInfo controllerInfo, Dictionary<string, object> state)
        {
            context.Response.ContentType = "application/json";
            context.Response.WriteAsync(@"{""result"": true}");
        }
        /// <summary>
        /// Удаление проекта
        /// </summary>
        private void Delete(HttpContext context, ControllerInfo controllerInfo, Dictionary<string, object> state)
        {
            context.Response.ContentType = "application/json";
            context.Response.WriteAsync(@"{""result"": true}");
        }

        /// <summary>
        /// Переименование проекта
        /// </summary>
        private void Rename(HttpContext context, ControllerInfo controllerInfo, Dictionary<string, object> state)
        {
            context.Response.ContentType = "application/json";
            context.Response.WriteAsync(@"{""result"": true}");
        }
    }
}
