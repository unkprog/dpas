using System;

namespace dpas.Net.Http
{
    public interface IHttpHandler
    {
        IHttpContext Context { get; }

        void Execute();
    }

    public class HttpHandler : IHttpHandler
    {
        public HttpHandler(IHttpContext context)
        {
            Context = context;
        }
        public IHttpContext Context { get; private set; }

        public void Execute()
        {
            //Response = new HttpResponse(Request);
            OnExecute();
            Context.Response.StreamClose();
        }

        public virtual void OnExecute()
        {

        }
    }
}
