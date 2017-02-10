using System;
using System.IO;
using dpas.Core.Extensions;
using dpas.Net.Http;
using dpas.Net.Http.Mvc;

namespace dpas.Service.Controller.Prj
{

    public class Navigation : IController
    {
        public virtual void Exec(IControllerContext context)
        {
            string curPage = context.ControllerInfo.CurrentPage;
            //if (curPage == "/curpage")
            //    curPage = context.State.GetString("curpage");

            //if (string.IsNullOrEmpty(curPage))
            //    curPage = "/index";

            //context.State["curpage"] = curPage;

            //if (!state.GetBool("IsAuthentificated"))
            //    curPage = "/auth";
            Page(context, curPage);
        }

        private void Page(IHttpContext context, string curPage)
        {
            string pathFile = string.Concat(Directory.GetCurrentDirectory(), "/content/mvc/view/prj", curPage, ".html");

            if (!File.Exists(pathFile))
            {
                PageNotFound(context);
                return;
            }

            context.Response.ContentType = "text/html";
            using (StreamReader sr = File.OpenText(pathFile))
            {
                string htmlLine = String.Empty;
                while ((htmlLine = sr.ReadLine()) != null)
                {
                    context.Response.Write(htmlLine);
                    context.Response.Write(Environment.NewLine);
                }
            }

            pathFile = string.Concat(Directory.GetCurrentDirectory(), "/content/mvc/controller/prj", curPage, ".js");

            if (!File.Exists(pathFile)) return;
            //context.Response.Write(string.Concat("<script type=", '"', "text/javascript", '"', "src=", '"', "mvc/controller", curPage, ".js", '"', "></script>", Environment.NewLine));
            context.Response.Write(Environment.NewLine);
            context.Response.Write(string.Concat("<script type=", '"', "text/javascript", '"', ">", Environment.NewLine));
            using (StreamReader sr = File.OpenText(pathFile))
            {
                string htmlLine = String.Empty;
                while ((htmlLine = sr.ReadLine()) != null)
                {
                    context.Response.Write(htmlLine);
                    context.Response.Write(Environment.NewLine);
                }
            }
            context.Response.Write(string.Concat("</script>", Environment.NewLine));

            //context.Response.Write(GACode);
        }

        private void PageNotFound(IHttpContext context)
        {
            context.Response.StatusCode = System.Net.HttpStatusCode.NotFound;
            context.Response.ContentType = "text/html";
            context.Response.Write(string.Concat("Страница <", context.Request.Url, "> не найдена"));
            context.Response.Write(Environment.NewLine);

        }
    }


}
