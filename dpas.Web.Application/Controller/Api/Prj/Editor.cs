using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using dpas.Net.Http.Mvc;


namespace dpas.Web.Application.Controller.Api.Prj
{
    public class Editor : dpas.Net.Http.Mvc.IController
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
