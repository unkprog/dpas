using System;
using System.Collections.Generic;
using dpas.Net.Http;


namespace dpas.Service.Protocol
{
    public partial class HttpProtocol
    {
        /// <summary>
        /// Словарь для хранения обработчиков запроса
        /// </summary>
        private static Dictionary<string, Func<HttpRequest, IHttpHandler>> httpHandlers = new Dictionary<string, Func<HttpRequest, IHttpHandler>>();

        /// <summary>
        /// Привязка пути (адреса) к обработчику
        /// </summary>
        /// <param name="path">Путь(адрес)</param>
        /// <param name="handler">Обработчик</param>
        public void MapPath(string path, Func<HttpRequest, IHttpHandler> handler)
        {
            if (httpHandlers.ContainsKey(path))
                httpHandlers[path] = handler;
            else
                httpHandlers.Add(path, handler);
        }

        /// <summary>
        /// Получение обработчика запроса
        /// </summary>
        /// <param name="request">Входящий запрос</param>
        /// <returns>Обработчик входящего запроса</returns>
        private static IHttpHandler GetHttpHandler(HttpRequest request)
        {
            IHttpHandler result = null;
            Func<HttpRequest, IHttpHandler> createHandler = null;
            if (httpHandlers.TryGetValue(request.Path, out createHandler))
                result = createHandler(request);
            else
                result = new HttpHandlerFile(request);
            return result;
        }
    }
}
