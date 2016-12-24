﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace dpas.Net.Http.Mvc.Controller.Api.Prj
{
    public class Editor : IController
    {
        public virtual void Exec(HttpContext context, ControllerInfo controllerInfo, Dictionary<string, object> state)
        {
            state["IsAuthentificated"] = true;

            context.Response.ContentType = "application/json";// application / json; charset = UTF - 8
            //context.Response.Headers.Add("charset", "UTF-8");
            context.Response.WriteAsync(@"{""result"": true}");
        }
    }
}
