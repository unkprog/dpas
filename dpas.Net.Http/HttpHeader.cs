using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpas.Net.Http
{
    public class HttpHeader : HttpHeaderBase
    {
        public const string AcceptEncoding  = "Accept-Encoding";
        public const string ContentEncoding = "Content-Encoding";
        public const string ContentType     = "Content-type";
        public const string ContentLength   = "Content-Length";
        public const string Connection      = "Connection";
        public const string SetCookie       = "Set-Cookie";
        public const string Cookie          = "Cookie";
        
        public string Method { get; internal set; }
        public string Source { get; internal set; }

        public override string ToString()
        {
            return string.Concat(Method, " ", Source, " ", base.ToString(), "\r\n");
        }
    }
}
