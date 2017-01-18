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
                context.Response.ContentType = "application/json";// application / json; charset = UTF - 8
                                                                  //context.Response.Headers.Add("charset", "UTF-8");
                context.Response.Write(@"{""result"": true}");
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

        private void ProjectToJson(StringBuilder sbJson, IProject project)
        {
            sbJson.Append("{\"Code\":");
            sbJson.Append(string.Concat("\"", project.Code, "\""));
            sbJson.Append(",\"Name\":");
            sbJson.Append(string.Concat("\"", project.Name, "\""));
            sbJson.Append(",\"Description\":");
            sbJson.Append(string.Concat("\"", project.Description, "\""));
            sbJson.Append("}");
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
                StringBuilder result = new StringBuilder("{\"result\": true, \"project\": ");
                ProjectToJson(result, project);
                result.Append("}");
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

                IProject findProject = ProjectManager.Manager.FindProject(parameters.GetString("prjName"));
                if (findProject != null)
                {
                    ProjectManager.Manager.Delete(findProject);
                    context.Response.ContentType = "application/json";
                    var result = new { result = true, project = findProject };
                    var strResult = Json.Serialize(result);
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

                IProject findProject = ProjectManager.Manager.Rename(parameters.GetString("prjOldName"), parameters.GetString("prjName"), parameters.GetString("prjDescription"));
                if (findProject != null)
                {
                    ProjectManager.Manager.Delete(findProject);
                    context.Response.ContentType = "application/json";
                    var result = new { result = true, project = findProject };
                    var strResult = Json.Serialize(result);
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(Json.Serialize(new { result = false, error = ex.Message }));
            }
        }
    }
}
