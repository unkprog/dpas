using System;
using dpas.Core;

namespace dpas.Net.Http
{
    public interface IHttpRequestResponse : IDisposable
    {
        IHttpParameters Parameters { get; }
        IHttpCookies Cookies { get; }
    }

    public class HttpRequestResponse : Disposable,  IHttpRequestResponse
    {
        public HttpRequestResponse()
        {
            Parameters = new HttpParameters();
            Cookies    = new HttpCookies();
        }

        public IHttpParameters Parameters { get; internal set; }
        public IHttpCookies    Cookies    { get; internal set; }
    }
}
