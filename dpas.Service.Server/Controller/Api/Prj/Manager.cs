using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using dpas.Core.Extensions;
using dpas.Service.Project;

namespace dpas.Net.Http.Mvc.Api.Prj
{
    public partial class Manager : IController
    {
        public virtual void Exec(IControllerContext context)
        {
            try
            {
                context.Response.ContentType = "application/json";

                if (context.ControllerInfo.Action == "/list")
                    List(context);
                else if (context.ControllerInfo.Action == "/current")
                    Current(context);
                else if (context.ControllerInfo.Action == "/create")
                    Create(context);
                else if (context.ControllerInfo.Action == "/delete")
                    Delete(context);
                else if (context.ControllerInfo.Action == "/rename")
                    Rename(context);
                else
                {
                    context.Response.ContentType = "application/json";
                    Json.Serialize(new { result = false, error = string.Concat("Команда <", context.ControllerInfo.Action, "> не поддерживается.") });
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(Json.Serialize(new { result = false, error = ex.Message }));
            }
        }

        /// <summary>
        /// Получение списка проектов
        /// </summary>
        private void List(IControllerContext context)
        {
            ProjectManager.Manager.Read();

            StringBuilder result = new StringBuilder("[");
            IProject project;
            IList<IProject> projects = ProjectManager.Manager.Projects;
            for (int i = 0, icount = projects.Count; i < icount; i++)
            {
                project = projects[i];
                if(i > 0)
                    result.Append(",");
                ProjectToJson(result, project, false);
            }
            result.Append("]");
            context.Response.Write(result.ToString());
        }

        private void Current(IControllerContext context)
        {
            string data = WebUtility.UrlDecode(context.Request.Content);
            Dictionary<string, object> parameters = (Dictionary<string, object>)Json.Parse(data);
            string prjCode = parameters.GetString("prjCode");
            if (string.IsNullOrEmpty(prjCode))
                prjCode = context.State.GetString("prjCurrent");
            else
                context.State.SetValue("prjCurrent", prjCode);
            context.Response.Write(Json.Serialize(new { result = true }));
        }

        private void ProjectToJson(StringBuilder resultJson, IProject project, bool appendProperty = true)
        {
            if(appendProperty)
                resultJson.Append("\"project\":");
            resultJson.Append("{\"Code\":");
            resultJson.Append(string.Concat("\"", project.Code, "\""));
            resultJson.Append(",\"Name\":");
            resultJson.Append(string.Concat("\"", project.Name, "\""));
            resultJson.Append(",\"Description\":");
            resultJson.Append(string.Concat("\"", project.Description, "\""));
            resultJson.Append("}");
        }

        private StringBuilder ResultProjectToJson(bool result, IProject project)
        {
            StringBuilder resultJson = new StringBuilder(string.Concat("{\"result\":", result.ToString().ToLower(), ","));
            ProjectToJson(resultJson, project);
            resultJson.Append("}");
            return resultJson;
        }

        /// <summary>
        /// Создание нового проекта
        /// </summary>
        private void Create(IControllerContext context)
        {
                string data = WebUtility.UrlDecode(context.Request.Content);
                Dictionary<string, object> parameters = (Dictionary<string, object>)Json.Parse(data);
                
                IProject project = ProjectManager.Manager.Create(parameters.GetString("prjName"), parameters.GetString("prjDescription"));
                ProjectManager.Manager.Save();
                StringBuilder result = ResultProjectToJson(true, project);
                context.Response.Write(result.ToString());
        }
        /// <summary>
        /// Удаление проекта
        /// </summary>
        private void Delete(IControllerContext context)
        {
            var data = WebUtility.UrlDecode(context.Request.Content);
            IHttpFormParameters parameters = HttpParser.ParseFormParameters(data);

            IProject project = ProjectManager.Manager.FindProject(parameters.GetString("prjName"));
            if (project != null)
            {
                ProjectManager.Manager.Delete(project);
                ProjectManager.Manager.Save();
                StringBuilder result = ResultProjectToJson(true, project);
                context.Response.Write(result.ToString());
            }
        }

        /// <summary>
        /// Переименование проекта
        /// </summary>
        private void Rename(IControllerContext context)
        {
            var data = WebUtility.UrlDecode(context.Request.Content);
            IHttpFormParameters parameters = HttpParser.ParseFormParameters(data);
            string prjOldName = parameters.GetString("prjOldName");
            IProject project = ProjectManager.Manager.Rename(parameters.GetString("prjOldName"), parameters.GetString("prjName"), parameters.GetString("prjDescription"));
            if (project != null)
            {
                ProjectManager.Manager.Delete(project);
                StringBuilder result = ResultProjectToJson(true, project);
                context.Response.Write(result.ToString());
            }
            else
                context.Response.Write(Json.Serialize(new { result = false, error = string.Concat("Проект ", prjOldName, " не найден.") }));
        }
    }
}
