using System;
using System.Linq;
using System.Collections.Generic;
using dpas.Core.Extensions;
using dpas.Service.Project;
using Microsoft.Extensions.Logging;
using dpas.Service.Controller.Api;

namespace dpas.Net.Http.Mvc.Api.Prj
{
    public partial class Manager
    {
        public static class Editor
        {
            private static Dictionary<string, Action<IControllerContext, Dictionary<string, object>>> commandHandlers;
            static Editor()
            {
                if (commandHandlers == null)
                {
                    commandHandlers = new Dictionary<string, Action<IControllerContext, Dictionary<string, object>>>();
                    commandHandlers.Add("prjtree", PrjTree);
                    commandHandlers.Add("readitem", ReadItem);
                    commandHandlers.Add("additem", AddItem);
                    commandHandlers.Add("saveitem", SaveItem);
                    //commandHandlers.Add("/create", Create);
                    //commandHandlers.Add("/delete", Delete);
                    //commandHandlers.Add("/rename", Rename);
                    //commandHandlers.Add("/editor", Editor);
                }
            }

            public static void Handle(IControllerContext context, Dictionary<string, object> parameters)
            {
                string command = parameters.GetString("command");

                Action<IControllerContext, Dictionary<string, object>> commandHandler;
                if (commandHandlers.TryGetValue(command, out commandHandler))
                    commandHandler(context, parameters);
                else
                    context.Response.Write(Json.Serialize(new { result = false, errorcode = ErrorCode.Command.IsNotSupported, error = ErrorCode.Command.GetErrorText(ErrorCode.Command.IsNotSupported, command) }));
            }

            private static IProject getProject(IControllerContext context)
            {
                string prjCode = context.State.GetString("prjCurrent");
                return getProject(prjCode);
            }

            public static IProject getProject(string prjCode)
            {
                IProject project = null;
                if (string.IsNullOrEmpty(prjCode))
                    throw new Project.Exception(Project.Exception.NotSelected);

                project = ProjectManager.Manager.FindProjectByCode(prjCode);
                if (project == null)
                    throw new Project.Exception(Project.Exception.NotFound, prjCode);

                return project;
            }

            private static Json.SerializeOptions getSerializeOptions()
            {
                Json.SerializeOptions options = new Json.SerializeOptions();
                options.ExcludeProperties.Add("Owner", false);
                options.ExcludeProperties.Add("IsDisposed", false);
                return options;
            }

            public static void PrjTree(IControllerContext context, Dictionary<string, object> parameters)
            {
                IProject project = getProject(context);

                var dataTreeProject = new object[] {
                    new {
                            Name = project.Name,
                            Path = project.Name,
                            Type = ProjectItem.Project,
                            Items = project.Items
                        }
                };
                
                var dataToSend = Json.Serialize(new { result = true, data = dataTreeProject }, getSerializeOptions());
                context.Response.Write(dataToSend);
            }

            public static void ReadItem(IControllerContext context, Dictionary<string, object> parameters)
            {
                IProject project = getProject(context);
                string path = parameters.GetString("path");

                IProjectItem item = ProjectManager.Manager.FindProjectItem(project, path);
                context.Response.Write(Json.Serialize(new { result = true, item = item }, getSerializeOptions()));
            }

            public static void AddItem(IControllerContext context, Dictionary<string, object> parameters)
            {
                IProject project = getProject(context);
                string parent = parameters.GetString("Parent"), name = parameters.GetString("Name"), description = parameters.GetString("Description");
                int type = parameters.GetInt32("Type");
                IProjectItem item = ProjectManager.Manager.ProjectAddItem(project, parent, type, name, description);
                context.Response.Write(Json.Serialize(new { result = true, data = item }, getSerializeOptions()));
            }

            public static void SaveItem(IControllerContext context, Dictionary<string, object> parameters)
            {
                IProject project = getProject(context);
                string path = parameters.GetString("path");

                Dictionary<string, object> data = (Dictionary<string, object>)parameters.GetValue("data");
                ProjectItem classItem = new ProjectItem(project);
                classItem.Read(data);

                ProjectManager.Manager.ProjectSaveItem(project, path, classItem);
                context.Response.Write(Json.Serialize(new { item = classItem, result = true }, getSerializeOptions()));
            }
        }
    }
}
