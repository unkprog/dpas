﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;

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

            app.UseStaticFiles();
            //app.UseWelcomePage("wwwroot\\Index.html");
            //app.us.UseStaticFiles();
            app.Run(async (context) =>
            {
                //await context.Response.WriteAsync("Hello World!");
                await Startup.Handle(context);
            });
        }

        public static Task Handle(HttpContext context)
        {

            Task result = new Task(() =>
            {
                string pathBase = context.Request.PathBase;
                string action = string.Empty;
                if (!string.IsNullOrEmpty(pathBase))
                {
                    int index = pathBase.LastIndexOf('/');
                    if(index > -1)
                    {
                        action = pathBase.Substring(index);
                    }
                }
               
              // context.Response.WriteAsync("Hello World!");
               

                bool isAjax = context.Request.Query.ContainsKey("ajax");
            string path = string.Concat(context.Request.PathBase);// context.Request.CurrentExecutionFilePath.Replace("/navigation", "").Replace("/curpage", ""));

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

                if (isAjax)
                    Page(context, action, path);
                else
                    ReadIndexHtml(context);
                    //context.Response.Redirect("/", true);
            });

            result.Start();

            return result;

        }


        private static void Page(HttpContext context, string action, string path)
        {
            string pathFile = string.Concat(path, "/", action, ".html");
            pathFile = string.Concat(System.IO.Directory.GetCurrentDirectory() + context.Request.PathBase, "/content/views", pathFile);
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
            //context.Response.Write(GACode);
        }

        private static void ReadIndexHtml(HttpContext context)
        {
            string pathFile = string.Concat(System.IO.Directory.GetCurrentDirectory(), "/wwwroot/Index.html");
            // System.Environment
            if (!File.Exists(pathFile)) return;

            //context.Response.ContentType = "text/html";
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
