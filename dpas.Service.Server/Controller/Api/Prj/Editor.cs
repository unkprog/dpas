using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace dpas.Net.Http.Mvc.Api.Prj
{
    public class Editor : IController
    {
        public virtual void Exec(ControllerContext context)
        {
            context.State["IsAuthentificated"] = true;

            
            context.Response.ContentType = "application/json";// application / json; charset = UTF - 8
            //context.Response.Headers.Add("charset", "UTF-8");
            context.Response.Write(@"{""result"": true}");
        }
    }
}
