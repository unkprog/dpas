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

    public interface IHttpRequest
    {
        HttpHeader Header { get; }
        IDictionary<string, string> Parameters  { get; }
        IDictionary<string, string> QueryString { get; }
        IDictionary<string, string> Cookies     { get; }

        string Url { get; }
        string Path { get; }
        HttpFile File { get; }

        int SupportCompression { get; }

        string Content { get; }

    }

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest()
        {
            Header      = new HttpHeader();
            Parameters  = new Dictionary<string, string>();
            QueryString = new Dictionary<string, string>();
            Cookies     = new Dictionary<string, string>();
            File        = new HttpFile();
        }
        public HttpHeader          Header      { get; internal set; }
        public IDictionary<string, string>  Parameters  { get; internal set; }
        public IDictionary<string, string>  QueryString { get; internal set; }
        public IDictionary<string, string>  Cookies     { get; internal set; }
        public string              Path        { get; internal set; }
        public HttpFile            File        { get; internal set; }
        public int                 SupportCompression   { get; internal set; }

        public string              Url        { get { return string.Concat(Path, QueryString); } }
        public string              Content    { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(Header);
            foreach (var param in Parameters) //.AllKeys)
            {
                result.Append(param.Key);
                result.Append(": ");
                result.Append(param.Value);
                result.Append("\r\n");
            }
            if (!string.IsNullOrEmpty(Content))
            {
                result.Append("\r\n\r\n");
                result.Append(Content);
            }
            return result.ToString();
        }
    }
}
