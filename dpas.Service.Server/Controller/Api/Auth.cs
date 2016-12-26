using System.Collections.Generic;

namespace dpas.Net.Http.Mvc.Api
{
    public class Auth : IController
    {
        public virtual void Exec(HttpContext context, ControllerInfo controllerInfo)
        {
            context.State["IsAuthentificated"] = true;

            if (controllerInfo.Action == "/login")
                Login(context, controllerInfo);
            else if (controllerInfo.Action == "/exit")
                Exit(context, controllerInfo);
            else
            {
                context.Response.ContentType = "application/json";// application / json; charset = UTF - 8
                                                                  //context.Response.Headers.Add("charset", "UTF-8");
                context.Response.Write(@"{""result"": true}");
            }
        }

        /// <summary>
        /// Вход в DPAS
        /// </summary>
        private void Login(HttpContext context, ControllerInfo controllerInfo)
        {
            context.State["IsAuthentificated"] = true;
            context.Response.ContentType = "application/json";// application / json; charset = UTF - 8
            //context.Response.Headers.Add("charset", "UTF-8");
            context.Response.Write(@"{""result"": true}");
        }

        /// <summary>
        /// Выход из DPAS
        /// </summary>
        private void Exit(HttpContext context, ControllerInfo controllerInfo)
        {
            context.State["IsAuthentificated"] = false;
            context.Response.ContentType = "application/json";// application / json; charset = UTF - 8
            //context.Response.Headers.Add("charset", "UTF-8");
            context.Response.Write(@"{""result"": true}");
        }
    }
}
