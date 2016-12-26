using System;
using System.Collections.Generic;
using dpas.Net.Http;
using dpas.Net.Http.Mvc;

namespace dpas.Service.Protocol
{
    public partial class HttpProtocol
    {
        /// <summary>
        /// Словарь для хранения обработчиков запроса
        /// </summary>
        private static Dictionary<string, Func<IHttpContext, IHttpHandler>> httpHandlers = new Dictionary<string, Func<IHttpContext, IHttpHandler>>();
        /// <summary>
        /// Словарь для хранения обработчиков MVC запроса
        /// </summary>
        private static Dictionary<string, Func<HttpRequest, IController>> mvcHandlers = new Dictionary<string, Func<HttpRequest, IController>>();


        /// <summary>
        /// Привязка пути (адреса) к обработчику
        /// </summary>
        /// <param name="path">Путь(адрес)</param>
        /// <param name="handler">Обработчик</param>
        public void MapPath(string path, Func<IHttpContext, IHttpHandler> handler)
        {
            if (httpHandlers.ContainsKey(path))
                httpHandlers[path] = handler;
            else
                httpHandlers.Add(path, handler);
        }

        /// <summary>
        /// Получение обработчика запроса
        /// </summary>
        /// <param name="context">Контекст выполнения запроса</param>
        /// <returns>Обработчик входящего запроса</returns>
        private static IHttpHandler GetHttpHandler(IHttpContext context)
        {
            IHttpHandler result = null;
            Func<IHttpContext, IHttpHandler> createHandler = null;
            if (httpHandlers.TryGetValue(context.Request.Path, out createHandler))
                result = createHandler(context);
            else
                result = new HttpHandlerFile(context);
            return result;
        }
    }
}
