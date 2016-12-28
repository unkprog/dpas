using System;
using System.Text;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace dpas.Net.Http
{
    public enum HttpCompress
    {
        None = 0,
        Gzip = 1,
        Deflate = 2
    }

    public interface IHttpRequest : IHttpRequestResponse
    {
        HttpHeader Header { get; }
        IHttpQueryString QueryString { get; }

        string Url { get; }
        string Path { get; }
        HttpFile File { get; }

        int SupportCompression { get; }

        string Content { get; }

    }

    public class HttpRequest : HttpRequestResponse, IHttpRequest
    {
        public HttpRequest() : base()
        {
            Header      = new HttpHeader();
            Parameters  = new HttpParameters();
            QueryString = new HttpQueryString();
            Cookies     = new HttpCookies();
            File        = new HttpFile();
        }
        public HttpHeader       Header      { get; internal set; }
        public IHttpQueryString QueryString { get; internal set; }
        public string           Path        { get; internal set; }
        public HttpFile         File        { get; internal set; }
        public int              SupportCompression   { get; internal set; }
        public string           Url        { get { return string.Concat(Header.Source, QueryString); } }
        public string           Content    { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(Header);
            if (Parameters.Count > 0)
            {
                result.Append(Parameters);
                result.Append(Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(Content))
            {
                result.Append(Environment.NewLine);
                result.Append(Environment.NewLine);
                result.Append(Content);
            }
            return result.ToString();
        }
    }
}
