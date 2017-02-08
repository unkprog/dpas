using System;
using System.Collections.Generic;

namespace dpas.Net.Http.Mvc.Api
{
    public class Auth : Controller
    {
        protected override void Init(Dictionary<string, Action<IControllerContext>> commandHandlers)
        {
            base.Init(commandHandlers);
            commandHandlers.Add("/login", Login);
            commandHandlers.Add("/exit", Exit);
        }

        /// <summary>
        /// Вход в DPAS
        /// </summary>
        private void Login(IControllerContext context)
        {
            context.State["IsAuthentificated"] = true;
            context.Response.ContentType = "application/json";
            context.Response.Write(@"{""result"": true}");
        }

        /// <summary>
        /// Выход из DPAS
        /// </summary>
        private void Exit(IControllerContext context)
        {
            context.State["IsAuthentificated"] = false;
            context.Response.ContentType = "application/json";
            //context.Response.Headers.Add("charset", "UTF-8");
            context.Response.Write(@"{""result"": true}");
        }
    }
}
