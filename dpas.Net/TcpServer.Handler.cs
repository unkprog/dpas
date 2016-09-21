using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace dpas.Net
{
    public partial class TcpServer
    {

        public event EventHandler OnStart;

        protected virtual void OnStartHandle()
        {
            if (OnStart != null)
                OnStart(this, EventArgs.Empty);
        }

        // This method is triggered when the accept socket completes an operation async
        // In the case of our accept socket, we are looking for a client connection to complete
        // This method is used to process the accept socket connection
        protected override void ProcessAccept(TcpSocketAsyncEventArgs e)
        {
           
#if DEBUG
            if (isLogging)
                WriteToLog("ProcessAccept");
#endif
            listenEvent.Set();
            
            // If the accept socket is connected to a client we will process it
            // otherwise nothing happens
            if (e.Socket.Connected)
            {
                try
                {
                    if (e.BytesTransferred > 0)
                    {
#if DEBUG
                        if (isLogging)
                            WriteToLog("ProcessAccept: acceptSocket.BytesTransferred > 0");
#endif
                        ProcessReceive(e);
                    }
                    else
                    {
#if DEBUG
                        if (isLogging)
                            WriteToLog("ProcessAccept: acceptSocket.ReceiveAsync()");
#endif
                        //e.SetBuffer(new byte[Settings.BufferSize], 0, Settings.BufferSize);
                        e.SetBuffer(new byte[10], 0, 10);
                        // Start a receive request and immediately check to see if the receive is already complete
                        // Otherwise OnIOCompleted will get called when the receive is complete
                        if (!e.Socket.ReceiveAsync(e))
                            ProcessReceive(e);
                    }

                }
                catch (Exception ex)
                {
                    SetException(ex, "TcpServer.ProcessAccept():");
                }

                //// Start the process again to wait for the next connection
                //StartProcessAccept(AsyncEventArgs);
            }
        }


//        protected override void ProcessReceive(TcpSocketAsyncEventArgs e)
//        {
//#if DEBUG
//            if (isLogging)
//                WriteToLog("ProcessReceive");
//#endif
//            base.ProcessReceive(e);
//        }

        protected override void OnReceiveHandle(TcpSocketAsyncEventArgs e)
        {
            base.OnReceiveHandle(e);
            if (e.BytesTransferred > 0 && e.Socket.Connected)
            {
                //Task<bool> send = SendAsync(e.ToArray(), e.Socket);
                bool send = Send(e.ToArray(), e.Socket);
                e.Clear();

                //if (!e.Socket.ReceiveAsync(e))
                //    ProcessReceive(e);
            }
            //else
            //{
            //    e.Clear();
            //    e.Socket.Shutdown(SocketShutdown.Both);
            //    e.Socket.Dispose();
            //    //poolEventArgs.Push(e);
            //}
        }

        protected override void ProcessSend(TcpSocketAsyncEventArgs e)
        {
#if DEBUG
            if (isLogging)
                WriteToLog("ProcessSend");
#endif
            base.ProcessSend(e);

            poolEventArgs.Push(e);
        }
    }
}
