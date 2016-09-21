using System;

namespace dpas.Net.Http
{
    public class HttpHeaderBase
    {
        public HttpHeaderBase()
        {
            Version = new HttpVersion();
        }
        
        private string _Protocol;
        public string Protocol
        {
            get { return _Protocol; }
            set
            {
                if (value != _Protocol) { _Protocol = value; updateVersion(); }
            }
        }


        public HttpVersion Version { get; internal set; }
        private void updateVersion()
        {
            Version.MajorVersion = 0;
            Version.MinorVersion = 0;
            string[] paramItems = string.IsNullOrEmpty(_Protocol) ? new string[] { "", "0.0" } : _Protocol.Split('/');
            if (paramItems.Length > 1)
            {
                paramItems = string.IsNullOrEmpty(paramItems[1]) ? new string[] { "0", "0" } : paramItems[1].Split('.');
                int version = 0;
                if (int.TryParse(paramItems[0], out version))
                    Version.MajorVersion = version;
                if (paramItems.Length > 1 && int.TryParse(paramItems[1], out version))
                    Version.MinorVersion = version;
            }
        }

        public bool ShouldKeepAlive
        {
            get
            {
                // true  ---> HTTP/1.1
                // false ---> < HTTP/1.1
                return (Version.MajorVersion > 0 && Version.MinorVersion > 0);
            }
        }

        public override string ToString()
        {
            return Protocol;
        }
    }
}
