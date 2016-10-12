using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using dpas.Net.Http.Mvc;
using dpas.Core.Extensions;

namespace dpas.Web.Application
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                //await context.Response.WriteAsync("Hello World!");
                await Handle(context);
            });
        }

        public static Task Handle(HttpContext context)
        {

            Task result = new Task(() =>
            {
                string dpasKey = context.Request.Cookies["dpas"];
                if (string.IsNullOrEmpty(dpasKey))
                {
                    dpasKey = Guid.NewGuid().ToString();
                    context.Response.Cookies.Append("dpas", dpasKey);
                }

                ControllerInfo controllerInfo = new ControllerInfo(string.Concat(context.Request.Path.Value, context.Request.QueryString));

                System.Diagnostics.Debug.WriteLine(string.Concat("Handle(HttpContext context): ", controllerInfo));

                var state = ControllerState.GetState(dpasKey);

                bool isAjax = context.Request.Query.ContainsKey("ajax");
                //string path = string.Concat(context.Request.PathBase);
                if (isAjax && controllerInfo.Controller == "/nav")
                {
                    string curPage = controllerInfo.Action;
                    if (curPage == "/curpage")
                        curPage = state.GetString("curpage");

                    if (string.IsNullOrEmpty(curPage))
                        curPage = "index";

                    state["curpage"] = curPage;

                    // context.Request.CurrentExecutionFilePath.Replace("/navigation", "").Replace("/curpage", ""));

                    //    //if (string.IsNullOrEmpty(action))
                    //    //{
                    //    //    object objPage = context.Session["curPage"];
                    //    //    if (objPage != null)
                    //    //        action = objPage.ToString();
                    //    //    else
                    //    //    {
                    //    //        action = "index";
                    //    //        path = string.Empty;
                    //    //        context.Session["curPage"] = action;
                    //    //        context.Session["curPath"] = path;
                    //    //    }
                    //    //}

                    //    if (string.IsNullOrEmpty(action) || action.ToLower() == "curpage")
                    //    {
                    //        object objPage = context.Session["curPage"];
                    //        action = objPage != null ? objPage.ToString() : string.Empty;
                    //        objPage = context.Session["curPath"];
                    //        path = objPage != null ? objPage.ToString() : string.Empty;
                    //    }

                    //    if (string.IsNullOrEmpty(action))
                    //    {
                    //        action = "index";
                    //        path = string.Empty;
                    //    }
                    //    path = path.Replace("/" + action, "");
                    //    context.Session["curPage"] = action;
                    //    context.Session["curPath"] = path;


                    Page(context, controllerInfo, curPage);
                }
                else
                    ReadFile(context, controllerInfo);
                //context.Response.Redirect("/", true);
            });

            result.Start();

            return result;

        }
       

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
            context.Response.WriteAsync(string.Concat("<script type=", '"', "text/javascript", '"', "src=", '"', "mvc/controller/index.js", '"', "></script>", Environment.NewLine));
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
