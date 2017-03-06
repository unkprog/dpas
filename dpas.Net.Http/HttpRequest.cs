using System;
using System.Text;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;

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

        bool ShouldKeepAlive { get; }
        int ContentLength { get; }

        MemoryStream ContentStream { get; }

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
            ContentStream     = new MemoryStream();
        }
        public HttpHeader       Header             { get; internal set; }
        public IHttpQueryString QueryString        { get; internal set; }
        public string           Path               { get; internal set; }
        public HttpFile         File               { get; internal set; }
        public int              SupportCompression { get; internal set; }
        public string           Url                { get { return string.Concat(Header.Source, QueryString); } }

        public bool             ShouldKeepAlive    { get; internal set; }
        public int              ContentLength      { get; internal set; }
        public MemoryStream     ContentStream      { get; internal set; }

        public string           Content            { get { return ContentStream != null && ContentStream.Length != 0 ? Encoding.UTF8.GetString(ContentStream.ToArray()) : string.Empty; } }

        public bool             IsContentContinueRead { get { return !(ContentLength == 0 || (ContentStream != null && ContentStream.Length == ContentLength)); } }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(Header);
            if (Parameters.Count > 0)
            {
                result.Append(Parameters);
                result.Append(Environment.NewLine);
            }
            //if (!string.IsNullOrEmpty(Content))
            //{
            //    result.Append(Environment.NewLine);
            //    result.Append(Environment.NewLine);
            //    result.Append(Content);
            //}
            return result.ToString();
        }

        protected override void Dispose(bool aDisposing)
        {
            if (aDisposing)
            {
                if (ContentStream != null) ContentStream.Dispose(); ContentStream = null;
            }
            base.Dispose(aDisposing);
        }
    }
}
