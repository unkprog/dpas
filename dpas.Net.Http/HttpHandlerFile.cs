﻿using System.IO;
using System.Collections.Generic;
using System.Net;
//using dpas.Core.IO.Debug;

namespace dpas.Net.Http
{
    public class HttpHandlerFile : HttpHandler
    {
        public HttpHandlerFile(HttpRequest request) : base(request)
        {
            Initialize();
        }

        private static string pathSources = string.Concat(System.IO.Directory.GetCurrentDirectory(), "/content").Replace('\\', '/');// Environment.CurrentDirectory.Replace('\\','/');

        public static void SetPathSources(string path)
        {
            pathSources = path.Replace('\\', '/');
        }

        //protected static Dictionary<string, string> 
        protected static Dictionary<string, string> mappingExtMime = null;
        protected virtual void Initialize()
        {
            if (mappingExtMime == null)
            {
                mappingExtMime = new Dictionary<string, string>();
                mappingExtMime.Add(FileExt.Htm, Mime.Text.Html);
                mappingExtMime.Add(FileExt.Html, Mime.Text.Html);
                mappingExtMime.Add(FileExt.Css, Mime.Text.Css);
                mappingExtMime.Add(FileExt.Js, Mime.Text.Javascript);
                mappingExtMime.Add(FileExt.Jpg, Mime.Image.Jpeg);
                mappingExtMime.Add(FileExt.Jpeg, Mime.Image.Jpeg);
                mappingExtMime.Add(FileExt.Png, Mime.Image.Png);
                mappingExtMime.Add(FileExt.Gif, Mime.Image.Gif);
            }
        }

        public override void OnExecute()
        {
            string filePath = string.Empty;// Request.Path + "/" + Request.File.ToString();
            string contentType = Request.Parameters[HttpHeader.ContentType];
            if (Request.Path == "/" || string.IsNullOrEmpty(Request.Path))
                filePath = string.Concat(pathSources, "/index.html");
            else
                filePath = string.Concat(pathSources, Request.Path, "/", Request.File);

            //BaseLog.WriteToLog(string.Format(filePath));

            if (!File.Exists(filePath))
            {
                Response.StatusCode = HttpStatusCode.NotFound;
                Response.Parameters.Add(HttpHeader.ContentType, string.Concat(Mime.Text.Html, "; charset=utf-8"));
                Response.StreamText.Write(string.Concat("<html><body><h1>", ((int)Response.StatusCode).ToString(), " ", ((HttpStatusCode)Response.StatusCode).ToString(), "</h1><div>Не найден файл</div><div></div></body></html>"));

                return;
            }

            HttpFile httpFile = new HttpFile(filePath);

            if (!mappingExtMime.TryGetValue(httpFile.FileExt, out contentType))
                contentType = Mime.Application.Unknown;


            Response.Parameters[HttpHeader.ContentType] = contentType;
            using (FileStream fileStream = File.Open(filePath, FileMode.Open))
            {
                int bufferSize = 4096;
                byte[] buffer = new byte[bufferSize];
                int readBytesCount = 0;
                while ((readBytesCount = fileStream.Read(buffer, 0, bufferSize)) > 0)
                    Response.Stream.Write(buffer, 0, readBytesCount);
            }
            base.OnExecute();
        }
    }
}
