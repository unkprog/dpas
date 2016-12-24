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
            HttpResponse response = null;
            try
            {
                //throw new Exception("Ооопс...");
                response = RequestHandle(request);
            }
            catch (Exception ex)
            {
                response = RequestError(request, ex, HttpStatusCode.InternalServerError);
            }
            SendResponse(e, request, response);
        }

        /// <summary>
        /// Отправка ответа
        /// </summary>
        /// <param name="socket">Сокет</param>
        /// <param name="response">Ответ</param>
        private void SendResponse(TcpSocket.TcpSocketAsyncEventArgs e, HttpRequest request, HttpResponse response)
        {
            byte[] responseData = null;

            using (MemoryStream ms = new MemoryStream())
            {
                response.Parameters.Add(HttpHeader.ContentLength, response.ContentLength.ToString());
                using (var sw = new StreamWriter(ms, Encoding.ASCII/*.UTF8*/, 4096, true))
                {
                    sw.Write(response.ToString());
                }
                if (response.ContentLength > 0)
                    response.StreamContentWriteTo(ms);
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

            response.Dispose();
        }


        /// <summary>
        /// Обработка запросов
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns>Ответ</returns>
        private static HttpResponse RequestHandle(HttpRequest request)
        {
            IHttpHandler handler = GetHttpHandler(request);
            if (handler == null) throw new Exception(string.Concat("Не найден обработчик: ", request.Path));
            handler.Execute();
            return handler.Response;
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
