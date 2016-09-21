using System;
using System.Threading;

namespace dpas.Core.Threading
{
    public class BaseThread : Disposable
    {
        private Thread thread;

        public BaseThread()
        {
            IsRunning = false;
        }

        public bool IsRunning { get; private set; }
        public void Start()
        {
            thread = new Thread(new ParameterizedThreadStart(DoRun));
            OnStart();
            thread.Start(this);
        }

        public void Stop()
        {
            IsRunning = false;
            OnStop();
            if (thread != null)
            {
                //// Request that threadLitener be stopped
                //thread.Abort();
                // Wait until threadLitener finishes. Join also has overloads
                // that take a millisecond interval or a TimeSpan object.
                thread.Join();
                thread = null;
            }
        }

        private void DoRun(object param)
        {
            BaseThread _this = param as BaseThread;
            if (_this != null)
                _this.Run();
        }

        public void Run()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                OnRun();
            }
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnStop()
        {
        }

        protected virtual void OnRun()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
            }
            base.Dispose(disposing);
        }
    }
}
