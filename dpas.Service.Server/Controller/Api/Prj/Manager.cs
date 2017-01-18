using System;
using System.Collections.Generic;
using dpas.Service.Project;
using System.Text;
using System.Net;
using dpas.Core.Extensions;

namespace dpas.Net.Http.Mvc.Api.Prj
{
    public class Manager : IController
    {
        public virtual void Exec(IControllerContext context)
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
                context.Response.ContentType = "application/json";
                Json.Serialize(new { result = false, error = string.Concat("Команда <", context.ControllerInfo.Action, "> не поддерживается.") });
            }
        }

        /// <summary>
        /// Получение списка проектов
        /// </summary>
        private void List(IControllerContext context)
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
                ProjectToJson(result, project);
            }
            result.Append("]");
            context.Response.Write(result.ToString());
        }

        private void ProjectToJson(StringBuilder resultJson, IProject project)
        {
            resultJson.Append("\"project\":{\"Code\":");
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
            try
            {
                string data = WebUtility.UrlDecode(context.Request.Content);
                Dictionary<string, object> parameters = (Dictionary<string, object>)Json.Parse(data);
                
                IProject project = ProjectManager.Manager.Create(parameters.GetString("prjName"), parameters.GetString("prjDescription"));
                ProjectManager.Manager.Save();
                context.Response.ContentType = "application/json";
                StringBuilder result = ResultProjectToJson(true, project);
                //context.State.SetValue("prjCurrent", newProject.Code);
                context.Response.Write(result.ToString());
            }
            catch(Exception ex)
            {
                context.Response.Write(Json.Serialize(new { result = false, error = ex.Message }));
            }
        }
        /// <summary>
        /// Удаление проекта
        /// </summary>
        private void Delete(IControllerContext context)
        {
            try
            {
                var data = WebUtility.UrlDecode(context.Request.Content);
                IHttpFormParameters parameters = HttpParser.ParseFormParameters(data);

                IProject project = ProjectManager.Manager.FindProject(parameters.GetString("prjName"));
                if (project != null)
                {
                    ProjectManager.Manager.Delete(project);
                    ProjectManager.Manager.Save();
                    context.Response.ContentType = "application/json";
                    StringBuilder result = ResultProjectToJson(true, project);
                    context.Response.Write(result.ToString());
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(Json.Serialize(new { result = false, error = ex.Message }));
            }
        }

        /// <summary>
        /// Переименование проекта
        /// </summary>
        private void Rename(IControllerContext context)
        {
            try
            {
                var data = WebUtility.UrlDecode(context.Request.Content);
                IHttpFormParameters parameters = HttpParser.ParseFormParameters(data);
                string prjOldName = parameters.GetString("prjOldName");
                IProject project = ProjectManager.Manager.Rename(parameters.GetString("prjOldName"), parameters.GetString("prjName"), parameters.GetString("prjDescription"));
                if (project != null)
                {
                    ProjectManager.Manager.Delete(project);
                    context.Response.ContentType = "application/json";
                    StringBuilder result = ResultProjectToJson(true, project);
                    context.Response.Write(result.ToString());
                }
                else
                    context.Response.Write(Json.Serialize(new { result = false, error = string.Concat("Проект ", prjOldName, " не найден.") }));
            }
            catch (Exception ex)
            {
                context.Response.Write(Json.Serialize(new { result = false, error = ex.Message }));
            }
        }
    }
}
