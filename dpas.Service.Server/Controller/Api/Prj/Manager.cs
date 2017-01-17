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
            try
            {
                string data = WebUtility.UrlDecode(context.Request.Content);
                object parameters = Json.Parse(data);
                //IHttpFormParameters parameters = HttpParser.ParseFormParameters(data);

                //IProject newProject = ProjectManager.Manager.Create(parameters.GetString("prjName"), parameters.GetString("prjDescription"));
                context.Response.ContentType = "application/json";
                var result = new { result = true };//, project = newProject };
                var strResult = Json.Serialize(result);
                //context.State.SetValue("prjCurrent", newProject.Code);
                context.Response.Write(strResult);
            }
            catch(Exception ex)
            {
                context.Response.Write(Json.Serialize(new { result = false, error = ex.Message }));
            }
        }
        /// <summary>
        /// Удаление проекта
        /// </summary>
        private void Delete(ControllerContext context)
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
        private void Rename(ControllerContext context)
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
