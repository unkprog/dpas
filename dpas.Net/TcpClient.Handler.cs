using System.Net.Sockets;

namespace dpas.Net
{
    public partial class TcpClient
    {
        public event SocketHandler OnConnect;

        protected override void ProcessError(TcpSocketAsyncEventArgs e)
        {
#if DEBUG
            if (isLogging)
                WriteToLog("ProcessError");
#endif
            if (e.LastOperation == SocketAsyncOperation.Connect) //e.SocketError == SocketError.ConnectionReset) // if (e.LastOperation == SocketAsyncOperation.Connect)
                Disconnect();
            base.ProcessError(e);
        }

        protected override void ProcessOther(TcpSocketAsyncEventArgs e)
        {
#if DEBUG
            if (isLogging)
                WriteToLog("ProcessOther");
#endif
            base.ProcessOther(e);
            poolEventArgs.Push(e);
        }

        /// <summary>
        /// Этот метод вызывается при завершении асинхронной операции подключения к серверу
        /// </summary>
        /// <param name="AsyncEventArgs"></param>
        protected override void ProcessConnect(TcpSocketAsyncEventArgs e)
        {
#if DEBUG
            if (isLogging)
                WriteToLog("ProcessConnect");
#endif
            base.ProcessConnect(e);
            SetState(TcpClentState.Connect);

            if (OnConnect != null)
                OnConnect(this, e);

            poolEventArgs.Push(e);
        }

        /// <summary>
        /// Этот метод вызывается при завершении асинхронной операции отключения от сервера
        /// </summary>
        /// <param name="AsyncEventArgs"></param>
        protected override void ProcessDisconnect(TcpSocketAsyncEventArgs e)
        {
#if DEBUG
            if (isLogging)
                WriteToLog("ProcessDisconnect");
#endif
            base.ProcessDisconnect(e);
            poolEventArgs.Push(e);
        }

        protected override void ProcessReceive(TcpSocketAsyncEventArgs e)
        {
#if DEBUG
            if (isLogging)
                WriteToLog("ProcessReceive");
#endif
            base.ProcessReceive(e);
        }

        protected override void ProcessSend(TcpSocketAsyncEventArgs e)
        {
#if DEBUG
            if (isLogging)
                WriteToLog("ProcessSend");
#endif
            base.ProcessSend(e);

#if DEBUG
            if (isLogging)
                WriteToLog("ProcessSend->e.Socket.ReceiveAsync");
#endif
            //Read data sent from the server
            e.Socket.ReceiveAsync(e);
        }
    }
}
