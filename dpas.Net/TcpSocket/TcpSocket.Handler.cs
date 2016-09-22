using System.Net.Sockets;

namespace dpas.Net
{
    public partial class TcpSocket
    {

        // This method is called when there is no more data to read from a connected client
        protected void OnSocketAsyncEventArgsCompleted(object sender, SocketAsyncEventArgs e)
        {
            TcpSocketAsyncEventArgs ee = (TcpSocketAsyncEventArgs)e;

#if DEBUG
            if (isLogging)
                WriteToLog("OnSocketAsyncEventArgsCompleted(Tobject sender, SocketAsyncEventArgs e): LastOperation = " + e.LastOperation + @", SocketError = " + e.SocketError);
#endif
            if (e.SocketError != SocketError.Success)
            {
                SetError(string.Concat("LastOperation =", e.LastOperation, ", SocketError=", e.SocketError), "TcpSocket.OnSocketAsyncEventArgsCompleted(object sender, SocketAsyncEventArgs e):");
                ProcessError(ee);
                return;
            }
            // Determine which type of operation just completed and call the associated handler.
            // We are only processing receives right now on this server.
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Accept    : this.ProcessAccept(ee); break;
                case SocketAsyncOperation.Connect   : this.ProcessConnect(ee); break;
                case SocketAsyncOperation.Disconnect: this.ProcessDisconnect(ee); break;
                case SocketAsyncOperation.Receive   : this.ProcessReceive(ee); break;
                case SocketAsyncOperation.Send      : this.ProcessSend(ee); break;
                default:
                    this.ProcessOther(ee); break;
            }
        }

        protected virtual void ProcessError(TcpSocketAsyncEventArgs e)
        {
            //if (e.LastOperation == SocketAsyncOperation.Receive)
            //{
                poolEventArgs.Push(e);
            //}
#if DEBUG
            if (isLogging)
                WriteToLog("ProcessError(TcpSocketAsyncEventArgs e): CountPool = " + poolEventArgs.CountEventsLock);
#endif
        }
        protected virtual void ProcessAccept(TcpSocketAsyncEventArgs e) { }
        protected virtual void ProcessConnect(TcpSocketAsyncEventArgs e) { }
        protected virtual void ProcessDisconnect(TcpSocketAsyncEventArgs e) { }
        // This method processes the read socket once it has a transaction
        protected virtual void ProcessReceive(TcpSocketAsyncEventArgs e)
        {
            // Если количество переданных байтов 0, то соединение было закрыто
            if (!(e.BytesTransferred > 0))
            {
#if DEBUG
                if (isLogging)
                    WriteToLog("ProcessReceive: Connection closed.");
#endif
                if (e.Socket.Connected)
                {
                    e.Socket.Shutdown(SocketShutdown.Both);
                    e.Socket.Dispose();
                }
                //e.Socket = null;
                poolEventArgs.Push(e);
                return;
            }

            e.Read();

#if DEBUG
            if (isLogging)
                WriteToLog(string.Concat("ProcessReceive ", e.BytesTransferred, " bytes"));
#endif

            // Прочитаны все данные, можем их теперь обработать
            if (e.Socket.Available == 0)
            {
                OnReceiveHandle(e);
                //poolEventArgs.Push(e);
                //e.Socket.Shutdown(SocketShutdown.Both);
                //e.Socket.Dispose();
                //poolEventArgs.Push(e);
            }
            //else // иначе продолжаем читать
            if (!e.Socket.ReceiveAsync(e))
                ProcessReceive(e);
        }

        // Called when a SendAsync operation completes
        protected virtual void ProcessSend(TcpSocketAsyncEventArgs e)
        {
#if DEBUG
            if (isLogging)
                WriteToLog(string.Concat("ProcessSend: Send ", e.BytesTransferred, " bytes"));
#endif
            OnSendHandle(e);
            //poolEventArgs.Push(e);
        }

        protected virtual void ProcessOther(TcpSocketAsyncEventArgs e) { }


        public event SocketHandler OnSend;
        public event SocketHandler OnReceive;

        private void DisposeEvents()
        {
            OnSend = null;
            OnReceive = null;
        }

        protected virtual void OnSendHandle(TcpSocketAsyncEventArgs e)
        {
            if (OnSend != null)
                OnSend(this, e);
        }

        protected virtual void OnReceiveHandle(TcpSocketAsyncEventArgs e)
        {
            if (OnReceive != null)
                OnReceive(this, e);
        }
    }
}
