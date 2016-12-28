using dpas.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace dpas.Net.Http
{
    public interface IHttpResponse : IHttpRequestResponse
    {
        HttpHeaderBase Header { get; }
        HttpStatusCode StatusCode { get; set; }
        StreamWriter StreamText { get; }

        Stream Stream { get; }


        /// <summary>
        /// Тип содержимого
        /// </summary>
        string ContentType { get; set; }

        long ContentLength { get; }
        byte[] ContentData { get; }

        void StreamContentWriteTo(Stream stream);

        void Write(string data);
        void StreamClose();
    }
    public class HttpResponse : HttpRequestResponse, IHttpResponse
    {
        public HttpResponse()
        {
            Header       = new HttpHeaderBase();
            memoryStream = new MemoryStream();
            StatusCode = HttpStatusCode.OK;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StreamClose();
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                    memoryStream = null;
                }
            }
            base.Dispose(disposing);
        }
        private MemoryStream memoryStream = null;
        private StreamWriter streamWriter = null;
        private Stream streamCompress = null;


        public HttpResponse(IHttpRequest request) : this()
        {
            Header.Protocol = request.Header.Protocol;
            Parameters.Add("Server", "DPAS");
            Parameters.Add("Connection", "close");
           // string accEnc = result.Parameters["Accept-Encoding"];
            //foreach (var param in request.Parameters.AllKeys)
            //{
            //    Parameters.Add(param, request.Parameters[param]);
            //}
            //Parameters[HttpHeader.Connection] = "closed";
            InitStream(request);
        }

        private void InitStream(IHttpRequest request)
        {
            if (request != null)
            {
                if ((request.SupportCompression & (int)HttpCompress.Gzip) == (int)HttpCompress.Gzip)
                {
                    Parameters[HttpHeader.ContentEncoding] = "gzip";
                    streamCompress = new GZipStream(memoryStream, CompressionMode.Compress, true);
                }
                else if ((request.SupportCompression & (int)HttpCompress.Deflate) == (int)HttpCompress.Deflate)
                {
                    Parameters[HttpHeader.ContentEncoding] = "deflate";
                    streamCompress = new DeflateStream(memoryStream, CompressionMode.Compress, true);
                }
            }
            if (streamCompress != null)
                streamWriter = new StreamWriter(streamCompress, Encoding.UTF8/*.ASCII*/, 4096, true);
            else
                streamWriter = new StreamWriter(memoryStream, Encoding.UTF8/*.ASCII*/, 4096, true);
        }

        public HttpHeaderBase      Header        { get; internal set; }
        public HttpStatusCode      StatusCode    { get; set; }
        public StreamWriter StreamText
        {
            get
            {
                if (streamWriter == null)
                    InitStream(null);
                return streamWriter;
            }
        }

        public Stream Stream
        {
            get
            {
                return streamCompress == null ? memoryStream : streamCompress;
            }
        }

        /// <summary>
        /// Тип содержимого
        /// </summary>
        public string ContentType
        {
            get { return Parameters[HttpHeader.ContentType]; }
            set { Parameters[HttpHeader.ContentType] = value; }
        }

        public long                ContentLength { get { return memoryStream.Length; } }
        public byte[]              ContentData   { get { return memoryStream.ToArray(); } }

        public void StreamContentWriteTo(Stream stream)
        {
            if (memoryStream != null)
                memoryStream.WriteTo(stream);
        }

        public void Write(string data)
        {
            byte[] _data = Encoding.UTF8.GetBytes(data);
            Stream.Write(_data, 0, _data.Length);
        }

        public void StreamClose()
        {
            if (streamWriter != null)
            {
                streamWriter.Dispose();
                streamWriter = null;
            }
            if (streamCompress != null)
            {
                streamCompress.Dispose();
                streamCompress = null;
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(Header);
            result.Append(" ");
            result.Append((int)StatusCode);
            result.Append(" ");
            result.Append(StatusCode.ToString());
            result.Append(Environment.NewLine);

            if (Cookies.Count > 0)
            {
                result.Append(Cookies);
                result.Append(Environment.NewLine);
            }

            if (Parameters.Count > 0)
            {
                result.Append(Parameters);
                result.Append(Environment.NewLine);
            }

            return result.ToString();
        }
    }
}
