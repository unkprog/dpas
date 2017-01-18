using System.Collections.Generic;

namespace dpas.Net.Http.Mvc.Api
{
    public class Auth : IController
    {
        public virtual void Exec(IControllerContext context)
        {
            context.State["IsAuthentificated"] = true;

            if (context.ControllerInfo.Action == "/login")
                Login(context);
            else if (context.ControllerInfo.Action == "/exit")
                Exit(context);
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
        private void Login(IControllerContext context)
        {
            context.State["IsAuthentificated"] = true;
            context.Response.ContentType = "application/json";// application / json; charset = UTF - 8
            //context.Response.Headers.Add("charset", "UTF-8");
            context.Response.Write(@"{""result"": true}");
        }

        /// <summary>
        /// Выход из DPAS
        /// </summary>
        private void Exit(IControllerContext context)
        {
            context.State["IsAuthentificated"] = false;
            context.Response.ContentType = "application/json";// application / json; charset = UTF - 8
            //context.Response.Headers.Add("charset", "UTF-8");
            context.Response.Write(@"{""result"": true}");
        }
    }
}
