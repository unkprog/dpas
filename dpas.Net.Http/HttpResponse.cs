﻿using dpas.Core;
using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace dpas.Net.Http
{
    public class HttpResponse : Disposable
    {
        public HttpResponse()
        {
            Header     = new HttpHeaderBase();
            Parameters = new NameValueCollection();
            StatusCode = HttpStatusCode.OK;
            memoryStream = new MemoryStream();
            
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


        public HttpResponse(HttpRequest request) : this()
        {
            Header.Protocol = request.Header.Protocol;
            foreach (var param in request.Parameters.AllKeys)
            {
                Parameters.Add(param, request.Parameters[param]);
            }
            //Parameters[HttpHeader.Connection] = "closed";
            InitStream(request);
        }

        private void InitStream(HttpRequest request)
        {
            if (request != null)
            {
                if ((request.SupportCompression & (int)HttpCompress.Gzip) == (int)HttpCompress.Gzip)
                {
                    this.Parameters[HttpHeader.ContentEncoding] = "gzip";
                    streamCompress = new GZipStream(memoryStream, CompressionMode.Compress, true);
                }
                else if ((request.SupportCompression & (int)HttpCompress.Deflate) == (int)HttpCompress.Deflate)
                {
                    this.Parameters[HttpHeader.ContentEncoding] = "deflate";
                    streamCompress = new DeflateStream(memoryStream, CompressionMode.Compress, true);
                }
            }
            if (streamCompress != null)
                streamWriter = new StreamWriter(streamCompress, Encoding.UTF8, 4096, true);
            else
                streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, 4096, true);
        }

        public HttpHeaderBase      Header        { get; internal set; }
        public HttpStatusCode      StatusCode    { get; set; }
        public NameValueCollection Parameters    { get; internal set; }
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

        public long                ContentLength { get { return memoryStream.Length; } }
        public byte[]              ContentData   { get { return memoryStream.ToArray(); } }

        public void StreamContentWriteTo(Stream stream)
        {
            if (memoryStream != null)
                memoryStream.WriteTo(stream);
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
            result.Append("\r\n");
            foreach (var param in Parameters.AllKeys)
            {
                result.Append(param);
                result.Append(": ");
                result.Append(Parameters[param]);
                result.Append("\r\n");
            }
            result.Append("\r\n");
            return result.ToString();
        }
    }
}