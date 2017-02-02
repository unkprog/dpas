using System;
using System.Net;
using System.IO;
using System.Text;
using dpas.Net;
using dpas.Net.Http;
using dpas.Net.Http.Mvc;
using dpas.Net.Http.Mvc.Api;
using dpas.Net.Http.Mvc.Api.Prj;
using dpas.Core.Extensions;
using static dpas.Service.DpasTcpServer;

namespace dpas.Service.Protocol
{
    public partial class HttpProtocol : IProtocol
    {
        public HttpProtocol()
        {
            _BufferSize = 1024;
        }

        TcpServer _server;
        public HttpProtocol(TcpServer server):this()
        {
            _server = server;
        }
        private int _BufferSize;
        public int BufferSize { get { return _BufferSize; } set { if (_BufferSize != value) _BufferSize = value; } }

        void IProtocol.Handle(TcpSocket.TcpSocketAsyncEventArgs e, byte[] data)
        {
            HttpRequest Request = e.UserToken as HttpRequest;
            if (Request == null)
                Request = HttpParser.ParseRequest(data);
            else
            {
                HttpParser.ParseContent(Request, data, 0, data.Length);
            }
            // ******************************************* //
            // КОСТЫЛЬ?!?!?!?!
            // ******************************************* //
            int contentLen = Request.Parameters.GetInt32(HttpHeader.ContentLength);
            if (string.IsNullOrEmpty(Request.Content) && contentLen != Request.Content.Length)
            {
                e.UserToken = Request;
                //int len = data.Length;
                return;
            }
            e.UserToken = null;
            // ******************************************* //
            // END КОСТЫЛЬ?!?!?!?!
            // ******************************************* //

            IControllerContext context = new ControllerContext(Request);
            try
            {
                string dpasKey;
                if (!context.Request.Cookies.TryGetValue("dpas", out dpasKey))
                {
                    dpasKey = Guid.NewGuid().ToString();
                    context.Response.Cookies.Add("dpas", dpasKey);
                }

                context.SetState(dpasKey);

                if (!RequestMvcHandle(context))
                    RequestHandle(context);
            }
            catch (Exception ex)
            {
                RequestError(context, ex, HttpStatusCode.InternalServerError);
            }
            SendResponse(e, context);

            if (!context.Request.Header.ShouldKeepAlive)
            {
                _server.CloseConnection(e);
               
            }

            context.Dispose();
        }

        /// <summary>
        /// Отправка ответа
        /// </summary>
        /// <param name="socket">Сокет</param>
        /// <param name="response">Ответ</param>
        private void SendResponse(TcpSocket.TcpSocketAsyncEventArgs e, IHttpContext context)
        {
            byte[] responseData = null;

            long contentLength = context.Response.ContentLength;

            using (MemoryStream ms = new MemoryStream())
            {
                if (contentLength > 0)
                    context.Response.Parameters.Add(HttpHeader.ContentLength, contentLength.ToString());

                if (context.Request.Header.ShouldKeepAlive)
                    context.Response.Parameters.Add(HttpHeader.Connection, "Keep-Alive");
                else
                    context.Response.Parameters.Add("Connection", "close");
                using (var sw = new StreamWriter(ms, Encoding.ASCII/*.UTF8*/, _BufferSize, true))
                {
                    sw.Write(context.Response.ToString());
                }
                if (contentLength > 0)
                    context.Response.StreamContentWriteTo(ms);
                responseData = ms.ToArray();
            }

            if (responseData != null)
            {
                var sendTask = e.Socket.Send(responseData);
            }

            
        }


        private static Navigation navigation = null;
        private static Auth auth = null;
        private static Manager manager = null;
        private static bool RequestMvcHandle(IControllerContext context)
        {
            bool result = false;
            if (context.ControllerInfo.Prefix == "/nav")
            {
                if (navigation == null)
                    navigation = new Navigation();
                navigation.Exec(context);
                result = true;
            }
            else if (context.ControllerInfo.Prefix == "/api")
            {
                if (context.ControllerInfo.Controller == "/auth")
                {
                    if (auth == null)
                        auth = new Auth();
                    auth.Exec(context);
                    result = true;
                }
                else if (context.ControllerInfo.Controller == "/prj")
                {
                    if (manager == null)
                        manager = new Manager();
                    manager.Exec(context);
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Обработка запросов
        /// </summary>
        /// <param name="context">Контекст выполнения запросаЗапрос</param>
        private static void RequestHandle(IHttpContext context)
        {
            IHttpHandler handler = GetHttpHandler(context);
            if (handler == null) throw new Exception(string.Concat("Не найден обработчик: ", context.Request.Path));
            handler.Execute();
        }

        /// <summary>
        /// Формирование ответа с ошибкой
        /// </summary>
        /// <param name="ex">Исключение</param>
        /// <param name="statusCode">Статус ошибки</param>
        private static void RequestError(IControllerContext context, Exception ex, HttpStatusCode statusCode)
        {
            context.Response.Parameters.Add(HttpHeader.ContentType, string.Concat(Mime.Text.Html, "; charset=utf-8"));
            context.Response.StreamText.Write(string.Concat("<html><body><h1>", ((int)statusCode).ToString(), " ", ((HttpStatusCode)statusCode).ToString(), "</h1><div>", ex.Message, "</div><div>", ex.StackTrace, "</div></body></html>"));
            context.Response.StreamClose();
        }
    }
}
