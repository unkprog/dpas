using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using dpas.Net.Http.Mvc;
using dpas.Core.Extensions;

namespace dpas.Web.Application.Controller
{
    public class Navigation : IController
    {
        public virtual void Exec(HttpContext context, ControllerInfo controllerInfo, Dictionary<string, object> state)
        {
            string curPage = controllerInfo.CurrrentPage;
            if (curPage == "/curpage")
                curPage = state.GetString("curpage");

            if (string.IsNullOrEmpty(curPage))
                curPage = "/index";

            state["curpage"] = curPage;

            if (!state.GetBool("IsAuthentificated"))
                curPage = "/auth";
            Page(context, controllerInfo, curPage);
        }

        private void Page(HttpContext context, ControllerInfo controllerInfo, string curPage)
        {
            string pathFile = string.Concat(System.IO.Directory.GetCurrentDirectory(), "/content/mvc/view", curPage, ".html");
            // System.Environment
            if (!File.Exists(pathFile)) return;

            context.Response.ContentType = "text/html";
            using (StreamReader sr = File.OpenText(pathFile))
            {
                string htmlLine = String.Empty;
                while ((htmlLine = sr.ReadLine()) != null)
                {
                    context.Response.WriteAsync(htmlLine);
                    context.Response.WriteAsync(Environment.NewLine);
                }
            }

            pathFile = string.Concat(System.IO.Directory.GetCurrentDirectory(), "/content/mvc/controller", curPage, ".js");

            if (!File.Exists(pathFile)) return;
            //context.Response.WriteAsync(string.Concat("<script type=", '"', "text/javascript", '"', "src=", '"', "mvc/controller", curPage, ".js", '"', "></script>", Environment.NewLine));
            //context.Response.WriteAsync(Environment.NewLine);
            context.Response.WriteAsync(string.Concat("<script type=", '"', "text/javascript", '"', ">", Environment.NewLine));
            using (StreamReader sr = File.OpenText(pathFile))
            {
                string htmlLine = String.Empty;
                while ((htmlLine = sr.ReadLine()) != null)
                {
                    context.Response.WriteAsync(htmlLine);
                    context.Response.WriteAsync(Environment.NewLine);
                }
            }
            context.Response.WriteAsync(string.Concat("</script>", Environment.NewLine));

            //context.Response.Write(GACode);
        }

    }
}
