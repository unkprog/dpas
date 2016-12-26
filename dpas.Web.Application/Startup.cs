using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using dpas.Net.Http.Mvc;
using System.Text;
using System.Net;

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

        private static string GetContent(HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            if (context.Request.ContentLength > 0)
            {
                long cl = (long)context.Request.ContentLength;
                int buffSize = 4096, offset = 0, readBytes;
                byte[] buffer = new byte[buffSize];
                while((readBytes=context.Request.Body.Read(buffer, offset, buffSize)) > 0)
                {
                    sb.Append(Encoding.UTF8.GetString(buffer, 0, readBytes));//.ASCII.GetString(buffer, 0, readBytes));
                }

            }
            return WebUtility.UrlDecode(sb.ToString());
        }
        public static Task Handle(HttpContext context)
        {

            Task result = new Task(() =>
            {
                //string dpasKey = context.Request.Cookies["dpas"];
                //if (string.IsNullOrEmpty(dpasKey))
                //{
                //    dpasKey = Guid.NewGuid().ToString();
                //    context.Response.Cookies.Append("dpas", dpasKey);
                //}

               
                //ControllerInfo controllerInfo = new ControllerInfo(string.Concat(context.Request.Path.Value, context.Request.QueryString), GetContent(context));
                //Dictionary<string, object> state = ControllerState.GetState(dpasKey);

                //bool isAjax = context.Request.Query.ContainsKey("ajax");

                //if (isAjax)
                //{
                //    if (controllerInfo.Prefix == "/nav")
                //        new Navigation().Exec(context, controllerInfo, state);
                    
                //}
                
                //else if (controllerInfo.Prefix == "/api")
                //{
                //    if(controllerInfo.Controller == "/auth")
                //        new Auth().Exec(context, controllerInfo, state);
                //    else if (controllerInfo.Controller == "/prj")
                //        new Manager().Exec(context, controllerInfo, state);
                //}
                //else
                //    ReadFile(context, controllerInfo);
            });

            result.Start();

            return result;

        }
       


        private static void ReadFile(HttpContext context, ControllerInfo controllerInfo)
        {
            string path = controllerInfo.Path;
            string pathFile = string.Concat(Directory.GetCurrentDirectory(), "/content", path == "/" ? "/Index.html" : path);
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
