﻿using System;

namespace dpas.Net.Http
{
    public interface IHttpHandler
    {
        HttpRequest  Request  { get; }
        HttpResponse Response { get; }

        void Execute();
    }

    public class HttpHandler : IHttpHandler
    {
        public HttpHandler(HttpRequest request)
        {
            Request = request;
        }

        public HttpRequest  Request  { get; private set; }
        public HttpResponse Response { get; private set; }

        public void Execute()
        {
            this.Response = new HttpResponse(Request);
            this.OnExecute();
            this.Response.StreamClose();
        }

        public virtual void OnExecute()
        {

        }
    }
}
