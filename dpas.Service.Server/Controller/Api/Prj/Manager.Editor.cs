using System;
using System.Collections.Generic;
using dpas.Core.Extensions;
using dpas.Service.Project;
using Microsoft.Extensions.Logging;

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
                    //commandHandlers.Add("/current", Current);
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
                    Json.Serialize(new { result = false, error = string.Concat("Команда редактора <", command, "> не поддерживается.") });
            }

            public static void PrjTree(IControllerContext context, Dictionary<string, object> parameters)
            {
                
                var dataTreeProject = new object[] { new
                {
                    id = 1,
                    name = "Проект",
                    path = "Проект",
                    type = 0,
                    children = new object[]{
                        new {
                        id= 2, name="Справочники", path="Проект/Справочники", type=1,
                         children = new object[] {
                            new { id=4, name="Базовый", path="Проект/Справочники/Базовый", type=3 },
                            new { id=5, name="ЕдИзм", path="Проект/Справочники/ЕдИзм", type=3 }
                        }
                    },
                        //        {
                        //            'id': 3, 'name': 'Данные', 'path': 'Проект/Данные', 'type': 2,
                        //            'children': [{ 'id': 6, 'name': 'Журнал1', 'path': 'Проект/Данные/Журнал1', 'type': 4 }, { 'id': 7, 'name': 'Журнал2', 'path': 'Проект/Данные/Журнал2', 'type': 4 }]
                        //        }]
                    }
                } };

                context.Response.Write(Json.Serialize(new { result = true, data = dataTreeProject }));
            }
        }
    }
}
