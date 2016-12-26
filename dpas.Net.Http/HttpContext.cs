using System;
using dpas.Core;
using System.Collections.Generic;

namespace dpas.Net.Http
{
    public interface IHttpContext : IDisposable
    {
        /// <summary>
        /// Входящий запрос
        /// </summary>
        IHttpRequest Request { get; }
        /// <summary>
        /// Исходящий ответ
        /// </summary>
        IHttpResponse Response { get; }

        Dictionary<string, object> State { get; }
    }
    public class HttpContext : Disposable, IHttpContext
    {
        public HttpContext(IHttpRequest aRequest)
        {
            Request = aRequest;
            Response = new HttpResponse(Request);
        }
        /// <summary>
        /// Входящий запрос
        /// </summary>
        public IHttpRequest Request { get; internal set; }
        /// <summary>
        /// Исходящий ответ
        /// </summary>
        public IHttpResponse Response { get; internal set; }

        public Dictionary<string, object> State
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override void Dispose(bool aDisposing)
        {
            if (aDisposing)
            {
                Request = null;
                if (Response != null)
                    Response.Dispose();
                Response = null;
            }
            base.Dispose(aDisposing);
        }
    }
}
