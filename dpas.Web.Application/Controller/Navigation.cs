using dpas.Net.Http.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace dpas.Web.Application.Controller
{
    public class Navigation
    {

        private static void Page(HttpContext context, ControllerInfo controllerInfo, string curPage)
        {
            string pathFile = string.Concat(System.IO.Directory.GetCurrentDirectory(), "/content/mvc/view/", curPage, ".html");
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

            pathFile = string.Concat(System.IO.Directory.GetCurrentDirectory(), "/content/mvc/controller/", curPage, ".js");

            if (!File.Exists(pathFile)) return;
            context.Response.WriteAsync(string.Concat("<script type=", '"', "text/javascript", '"', "src=", '"', "mvc/controller/", curPage, ".js", '"', "></script>", Environment.NewLine));
            context.Response.WriteAsync(Environment.NewLine);

            //context.Response.Write(GACode);
        }

        private static void ReadFile(HttpContext context, ControllerInfo controllerInfo)
        {
            string path = controllerInfo.Path;
            string pathFile = string.Concat(System.IO.Directory.GetCurrentDirectory(), "/content", path == "/" ? "/Index.html" : path);
            // System.Environment
            if (!File.Exists(pathFile)) return;

            if (path.StartsWith("/css", StringComparison.OrdinalIgnoreCase)) context.Response.ContentType = "text/css";
            else context.Response.ContentType = "text/html";
            using (StreamReader sr = File.OpenText(pathFile))
            {
                string htmlLine = String.Empty;
                while ((htmlLine = sr.ReadLine()) != null)
                {
                    context.Response.WriteAsync(htmlLine);
                    context.Response.WriteAsync(Environment.NewLine);
                }
            }
        }
    }
}
