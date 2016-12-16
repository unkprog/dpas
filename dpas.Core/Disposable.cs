using System;
using System.ComponentModel;

namespace dpas.Core
{
    public class Disposable : IDisposable
    {
        public Disposable()
        {
            IsDisposed = false;
        }
    #region IDisposable Members
        [Browsable(false)]
        public bool IsDisposed { get; private set; }

        private event EventHandler disposed;

        /// <summary>
        /// Occurs when this instance is disposed.
        /// </summary>
        public event System.EventHandler Disposed { add { disposed += value; } remove { disposed -= value; } }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                Dispose(true);
                System.GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="aDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool aDisposing)
        {
            try
            {
                if (aDisposing)
                {
                    System.EventHandler handler = disposed;
                    if (handler != null)
                    {
                        handler(this, System.EventArgs.Empty);
                        handler = null;
                    }
                }
            }
            finally
            {
                IsDisposed = true;
            }
        }
    #endregion IDisposable Members
    }
}
