using System.IO;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Logging;
//using dpas.Core.IO.Debug;

namespace dpas.Net.Http
{
    public class HttpHandlerFile : HttpHandler
    {
        private static ILogger logger;
        public HttpHandlerFile(IHttpContext context) : base(context)
        {
            if (logger == null)
            {
                ILoggerFactory loggerFactory = new LoggerFactory().AddConsole(true);
                logger = loggerFactory.CreateLogger<HttpHandlerFile>();
            }
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
            string contentType;
            if (!Context.Request.Parameters.TryGetValue(HttpHeader.ContentType, out contentType))
                contentType = string.Empty;
            if (Context.Request.Path == "/" || string.IsNullOrEmpty(Context.Request.Path))
                filePath = string.Concat(pathSources, "/index.html");
            else
            {
                string file = WebUtility.UrlDecode(Context.Request.File.ToString());
                // file = string.Concat(string.IsNullOrEmpty(file) ? string.Empty : "/", Context.Request.File
                filePath = string.Concat(pathSources, WebUtility.UrlDecode(Context.Request.Path), string.IsNullOrEmpty(file) ? string.Empty : "/", file);
            }

            logger.LogInformation(string.Concat(System.Environment.NewLine, "Путь запроса: ", Context.Request.Path, System.Environment.NewLine, "Файл для чтения: ", filePath));
            //BaseLog.WriteToLog(string.Format(filePath));

            filePath = filePath.Replace('/', '\\');

            if (!File.Exists(filePath))
            {
                //Context.Response.StatusCode = HttpStatusCode.NotFound;
                // Context.Response.Parameters.Add(HttpHeader.ContentType, string.Concat(Mime.Text.Html, "; charset=utf-8"));
                //Context.Response.StreamText.Write(string.Concat("<html><body><h1>", ((int)Context.Response.StatusCode).ToString(), " ", (Context.Response.StatusCode).ToString(), "</h1><div>Не найден файл</div><div></div></body></html>"));
                logger.LogError(string.Concat(System.Environment.NewLine, "Файл для чтения не найден: ", filePath));
            }
            else
            {
                HttpFile httpFile = new HttpFile(filePath);

                if (!mappingExtMime.TryGetValue(httpFile.FileExt, out contentType))
                    contentType = Mime.Application.Unknown;

                Context.Response.Parameters[HttpHeader.ContentType] = contentType;
                using (FileStream fileStream = File.Open(filePath, FileMode.Open))
                {
                    int bufferSize = 4096;
                    byte[] buffer = new byte[bufferSize];
                    int readBytesCount = 0;
                    while ((readBytesCount = fileStream.Read(buffer, 0, bufferSize)) > 0)
                        Context.Response.Stream.Write(buffer, 0, readBytesCount);
                }
            }
            base.OnExecute();
        }
    }
}
