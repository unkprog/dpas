using System;
using System.Net;
using System.IO;
using System.Text;
using dpas.Net;
using dpas.Net.Http;
using System.Net.Sockets;
using static dpas.Service.DpasTcpServer;

namespace dpas.Service.Protocol
{
    public partial class HttpProtocol : IProtocol
    {
        public HttpProtocol()
        {
            BufferSize = 1024;
        }

        public int BufferSize { get; set; }

        void IProtocol.Handle(TcpSocket.TcpSocketAsyncEventArgs e, byte[] data)
        {
           
            // HttpParser request = new 
            HttpRequest request = HttpParser.ParseRequest(data);
            HttpContext context = new HttpContext(request);
            HttpResponse response = null;
            try
            {
                //ControllerInfo controllerInfo = new ControllerInfo(string.Concat(context.Request.Path.Value, context.Request.QueryString), GetContent(context));
                //Dictionary<string, object> state = ControllerState.GetState(dpasKey);

                //throw new Exception("Ооопс...");
                RequestHandle(context);
            }
            catch (Exception ex)
            {
                response = RequestError(request, ex, HttpStatusCode.InternalServerError);
            }
            SendResponse(e, context);
        }

        /// <summary>
        /// Отправка ответа
        /// </summary>
        /// <param name="socket">Сокет</param>
        /// <param name="response">Ответ</param>
        private void SendResponse(TcpSocket.TcpSocketAsyncEventArgs e, IHttpContext context)
        {
            byte[] responseData = null;

            using (MemoryStream ms = new MemoryStream())
            {
                context.Response.Parameters.Add(HttpHeader.ContentLength, context.Response.ContentLength.ToString());
                using (var sw = new StreamWriter(ms, Encoding.ASCII/*.UTF8*/, 4096, true))
                {
                    sw.Write(context.Response.ToString());
                }
                if (context.Response.ContentLength > 0)
                    context.Response.StreamContentWriteTo(ms);
                responseData = ms.ToArray();
            }

            if (responseData != null)
            {
                var sendTask = e.Socket.Send(responseData);
            }

            //if (!response.Header.ShouldKeepAlive)
            {
                e.CloseSocket();
                e.Socket.Dispose();
            }

            context.Dispose();
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
        /// <returns>Ответ с ошибкой</returns>
        private static HttpResponse RequestError(HttpRequest request, Exception ex, HttpStatusCode statusCode)
        {
            HttpResponse result = new HttpResponse(request);
            result.Parameters.Add(HttpHeader.ContentType, string.Concat(Mime.Text.Html, "; charset=utf-8"));
            result.StreamText.Write(string.Concat("<html><body><h1>", ((int)statusCode).ToString(), " ", ((HttpStatusCode)statusCode).ToString(), "</h1><div>", ex.Message, "</div><div>", ex.StackTrace, "</div></body></html>"));
            result.StreamClose();
            return result;
        }
    }
}
