using System;
using System.Text;
using System.Collections.Specialized;

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
        NameValueCollection Parameters { get; }
        string Path { get; }
        HttpFile File { get; }
        NameValueCollection QueryString { get; }

        int SupportCompression { get; }

        string Content { get; }

    }

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest()
        {
            Header      = new HttpHeader();
            Parameters  = new NameValueCollection();
            QueryString = new NameValueCollection();
            File        = new HttpFile();
        }
        public HttpHeader          Header      { get; internal set; }
        public NameValueCollection Parameters  { get; internal set; }
        public string              Path        { get; internal set; }
        public HttpFile            File        { get; internal set; }
        public NameValueCollection QueryString { get; internal set; }

        public int                 SupportCompression { get; internal set; }


        public string              Content    { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(Header);
            foreach (var param in Parameters.AllKeys)
            {
                result.Append(param);
                result.Append(": ");
                result.Append(Parameters[param]);
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
