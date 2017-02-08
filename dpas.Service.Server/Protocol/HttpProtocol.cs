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
            if (Request.ContentLength != Request.Content.Length)
            {
                e.UserToken = Request;
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

            if (!context.Request.Header.IsSupportShouldKeepAlive)
            {
                _server.CloseConnection(e);
            }

            context.Dispose();
        }

        /// <summary>
        /// Отправка ответа
        /// </summary>
        /// <param name="e">Контекст асинхронного сокета</param>
        /// <param name="context">Контекст выполнения запроса</param>
        private void SendResponse(TcpSocket.TcpSocketAsyncEventArgs e, IHttpContext context)
        {
            byte[] responseData = null;

            long contentLength = context.Response.ContentLength;

            using (MemoryStream ms = new MemoryStream())
            {
                if (contentLength > 0)
                    context.Response.Parameters.Add(HttpHeader.ContentLength, contentLength.ToString());
                context.Response.Parameters.Add(HttpHeader.Connection, context.Request.Header.IsSupportShouldKeepAlive && context.Request.ShouldKeepAlive ? HttpHeader.ConnectionKeepAlive : HttpHeader.ConnectionClose);

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
        private static bool RequestNavigation(IControllerContext context)
        {
            if (navigation == null)
                navigation = new Navigation();
            navigation.Exec(context);
            return true;
        }

        private static Auth auth = null;
        private static Manager manager = null;

        private static bool RequestApi(IControllerContext context)
        {
            if (context.ControllerInfo.Controller == "/auth")
            {
                if (auth == null)
                    auth = new Auth();
                auth.Exec(context);
               return true;
            }
            else if (context.ControllerInfo.Controller == "/prj")
            {
                if (manager == null)
                    manager = new Manager();
                manager.Exec(context);
                return true;
            }
            return false;
        }

        private static bool RequestMvcHandle(IControllerContext context)
        {
                 if (context.ControllerInfo.Prefix == "/nav") return RequestNavigation(context);
            else if (context.ControllerInfo.Prefix == "/api") return RequestApi(context);
            return false;
        }

        /// <summary>
        /// Обработка запросов
        /// </summary>
        /// <param name="context">Контекст выполнения запроса</param>
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
            context.Response.StreamText.Write(string.Concat("<html><body><h1>", ((int)statusCode).ToString(), " ", ((HttpStatusCode)statusCode).ToString(), "</h1><div>", ex.ToString(), "</div></body></html>")); //<div>", ex.StackTrace, "</div>
            context.Response.StreamClose();
        }
    }
}
