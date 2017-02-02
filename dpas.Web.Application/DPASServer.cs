
using System;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace dpas.Web.Application
{
    public class DPASServerOptions
    {

    }

    public class DPASServer : IServer
    {
        //private readonly IApplicationLifetime _applicationLifetime;
        private readonly ILoggerFactory _loggerFactory;

        public DPASServer(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            Features = new FeatureCollection();
            Features.Set<IHttpRequestFeature>(new HttpRequestFeature());
            Features.Set<IHttpResponseFeature>(new HttpResponseFeature());

        }

        public IFeatureCollection Features { get; } 

        public void Start<TContext>(IHttpApplication<TContext> application)
        {
            try
            {
                using (Service.IServer engine = new Service.Server(_loggerFactory))
                {
                    engine.Settings.IsLogging = true;
                    engine.Settings.Port = 5000;
                    engine.Start();
                }
               
            }
            catch (Exception ex)
            {
                //_logger.LogCritical(0, ex, "Unable to start Kestrel.");
                Dispose();
                throw ex;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DPASServer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
