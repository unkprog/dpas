using System;
using System.IO;
using dpas.Core.Extensions;
using System.Text;

namespace dpas.Net.Http.Mvc
{
    public class Navigation : IController
    {
        protected string rootPathView = "/mvc/view";
        protected string rootPathController = "/mvc/controller";
        public virtual void Exec(IControllerContext context)
        {
            //if (!state.GetBool("IsAuthentificated"))
            //    curPage = "/auth";

            string curPage = GetCurrentPage(context);
            
            Page(context, curPage);
        }

        protected virtual string GetCurrentPage(IControllerContext context)
        {
            string curPage = context.ControllerInfo.CurrentPage;
            if (curPage == "/curpage")
                curPage = context.State.GetString("curpage");

            if (string.IsNullOrEmpty(curPage))
                curPage = "/index";

            context.State["curpage"] = curPage;
            return curPage;
        }

        private void Page(IHttpContext context, string curPage)
        {
            string pathFile = string.Concat(Directory.GetCurrentDirectory(), "/content", rootPathView, curPage, ".html");
           
            if (!File.Exists(pathFile))
            {
                PageNotFound(context);
                return;
            }

            StringBuilder sbView = new StringBuilder();
            using (StreamReader sr = File.OpenText(pathFile))
            {
                string htmlLine = String.Empty;
                while ((htmlLine = sr.ReadLine()) != null)
                {
                    sbView.AppendLine(htmlLine);
                    //context.Response.Write(Json.Serialize(htmlLine));
                    //context.Response.Write(Environment.NewLine);
                }
            }

            pathFile = string.Concat(Directory.GetCurrentDirectory(), "/content", rootPathController, curPage, ".js");

            if (File.Exists(pathFile))
                sbView.AppendLine(string.Concat("<script type=\"text/javascript\" src=\"", rootPathController, curPage, ".js\"></script>"));

            context.Response.ContentType = Mime.Application.Json;
            context.Response.Write("{ \"view\": ");
            context.Response.Write(Json.Serialize(sbView.ToString()));
            context.Response.Write(Environment.NewLine);
            context.Response.Write(", \"result\": true");
            context.Response.Write(Environment.NewLine);
            context.Response.Write("}");
        }

        private void PageNotFound(IHttpContext context)
        {
            context.Response.ContentType = Mime.Application.Json;
            context.Response.Write(Json.Serialize(new { view = string.Concat("Страница <i><b>", context.Request.Url, "</b></i> не найдена."), result = false, error = string.Concat("Страница <i>", context.Request.Url, "</i> не найдена.") }));

            //context.Response.StatusCode = System.Net.HttpStatusCode.NotFound;
            //context.Response.ContentType = Mime.Text.Html;
            //context.Response.Write(string.Concat("Страница ", context.Request.Url, " не найдена"));
            //context.Response.Write(Environment.NewLine);
        }
    }
}
