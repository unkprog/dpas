using System;
using System.Collections.Generic;
using dpas.Core;
using System.Net.Sockets;
using System.Threading;

namespace dpas.Net
{
    public partial class TcpSocket
    {
        public class TcpSocketAsyncEventArgs : SocketAsyncEventArgs
        {
            public bool IsClosed { get; internal set; }
            public TcpSocketAsyncEventArgs() : base()
            {
                data = new List<byte[]>();
            }

            public TcpSocketAsyncEventArgs(int bufferSize) : this()
            {
                SetBuffer(new byte[bufferSize], 0, bufferSize);
            }

            private List<byte[]> data;

            public Socket Socket { get { return AcceptSocket == null ? ConnectSocket : AcceptSocket; } }

            // TODO: Здесь сделать оптимизацию по использованию памяти!!!
            public bool Read()
            {
                IsClosed = false;
                int bytecount = BytesTransferred;
                byte[] buffer = new byte[bytecount];
                data.Add(buffer);
                Array.Copy(Buffer, Offset, buffer, 0, bytecount);
                return true;
            }

            public byte[] ToArray()
            {
                byte[] result = null;
                int countRead = 0, bytesRead;
                int i, icount = data.Count;
                for (i = 0; i < icount; i++)
                    countRead += data[i].Length;
                result = new byte[countRead];
                countRead = 0;
                byte[] buffer;
                for (i = 0; i < icount; i++)
                {
                    buffer = data[i];
                    bytesRead = buffer.Length;
                    Array.Copy(buffer, 0, result, countRead, bytesRead);
                    countRead += bytesRead;
                }
                return result;
            }

            public new void Dispose()
            {
                AcceptSocket = null;
                Clear();
                data = null;
                UserToken = null;
                base.Dispose();
            }

            public void Clear()
            {
                if (data != null)
                    data.Clear();
            }

            public void CloseSocket()
            {
                IsClosed = true;
                if (Socket!= null && Socket.Connected)
                {
                    try
                    {
                        Socket.Shutdown(SocketShutdown.Both);
                    }
                    catch (Exception) { }
                }
            }
        }

        /// <summary>
        /// Класс PoolSocketAsyncEventArgs
        /// Содержит имплементацию стека объектов типа SocketAsyncEventArgs.
        /// Используется при работе с асинхронными сокетами.
        /// </summary>
        public sealed class PoolSocketAsyncEventArgs : Disposable
        {
            private TcpSocket owner;
            private Stack<TcpSocketAsyncEventArgs>     stackSocketAsyncEventArgs;
            private EventHandler<SocketAsyncEventArgs> onSocketAsyncEventArgsCompleted;
            private Func<TcpSocketAsyncEventArgs>      newSocketAsyncEventArgs;
            private Action<TcpSocketAsyncEventArgs>    disposeSocketAsyncEventArgs;
            internal bool isLogging = false;
            internal string loggingTag = "PoolSocketAsyncEventArgs";

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="capacity">Начальное количество объектов</param>
            /// <param name="onSocketAsyncEventArgsCompleted">Коллбэк при работе с асинхронными сокетми</param>
            /// <param name="newSocketAsyncEventArgs">Функция, вызываемая при создании нового объекта при нехватке в пуле</param>
            /// <param name="disposeSocketAsyncEventArgs">Функция, вызываемая при освобождении объекта, размещенного в пуле</param>
            public PoolSocketAsyncEventArgs(TcpSocket owner, int capacity, EventHandler<SocketAsyncEventArgs> onSocketAsyncEventArgsCompleted, Func<TcpSocketAsyncEventArgs> newSocketAsyncEventArgs, Action<TcpSocketAsyncEventArgs> disposeSocketAsyncEventArgs)
            {
                this.owner = owner;
                this.onSocketAsyncEventArgsCompleted = onSocketAsyncEventArgsCompleted;
                this.newSocketAsyncEventArgs         = newSocketAsyncEventArgs;
                this.disposeSocketAsyncEventArgs     = disposeSocketAsyncEventArgs;
                stackSocketAsyncEventArgs = new Stack<TcpSocketAsyncEventArgs>(capacity);

                for (int i = 0; i < capacity; i++)
                    stackSocketAsyncEventArgs.Push(NewSocketAsyncEventArgs());
            }

            public void WriteToLog(string data)
            {
                owner.WriteToLog("PoolSocketAsyncEventArgs: " + data);
            }
            
            /// <summary>
            /// Внутренняя функция создания нового объекта аргументов для асинхронного сокета
            /// </summary>
            /// <returns></returns>
            private TcpSocketAsyncEventArgs NewSocketAsyncEventArgs()
            {
                TcpSocketAsyncEventArgs e = (newSocketAsyncEventArgs != null ? newSocketAsyncEventArgs() : new TcpSocketAsyncEventArgs());
                e.Completed += onSocketAsyncEventArgsCompleted;
                return e;
            }

            private int countEventsLock = 0;

            public int CountEventsLock {  get { return countEventsLock; } }

            /// <summary>
            /// Извлечение объекта аргументов для асинхронного сокета из стека
            /// </summary>
            /// <returns></returns>
            public TcpSocketAsyncEventArgs Pop()
            {
                TcpSocketAsyncEventArgs result = null;
                //We are locking the stack, but we could probably use a ConcurrentStack if
                // we wanted to be fancy
                lock (stackSocketAsyncEventArgs)
                {
                    if (stackSocketAsyncEventArgs.Count > 0)
                    {
                        result = stackSocketAsyncEventArgs.Pop();
                        result.Clear();
                        Interlocked.Increment(ref countEventsLock);
                    }
                    else if (newSocketAsyncEventArgs != null)
                    {
                        result = newSocketAsyncEventArgs();
                        Interlocked.Increment(ref countEventsLock);
                    }
#if DEBUG
                    if (isLogging)
                        WriteToLog("PoolSocketAsyncEventArgs.Pop(): CountLock = " + CountEventsLock);
#endif
                }
                if (result != null)
                {
                    result.AcceptSocket = null;
                    result.SocketError =  SocketError.Success;
                    result.RemoteEndPoint = null;
                    result.UserToken = null;
                }
                return result;
            }

            /// <summary>
            /// Возвращение объекта аргументов для асинхронного сокета в стек
            /// </summary>
            /// <param name="item"></param>
            public void Push(TcpSocketAsyncEventArgs e)
            {
                if (e == null)
                {
                    throw new ArgumentNullException("Cannot add null object to socket stack");
                }

                lock (stackSocketAsyncEventArgs)
                {
                    Interlocked.Decrement(ref countEventsLock);
                    DisposeItem(e);
                    stackSocketAsyncEventArgs.Push(NewSocketAsyncEventArgs());
#if DEBUG
                    if (isLogging)
                        WriteToLog("PoolSocketAsyncEventArgs.Push(): CountLock = " + CountEventsLock);
#endif
                }

            }

            private void DisposeItem(TcpSocketAsyncEventArgs e)
            {
                if (disposeSocketAsyncEventArgs != null)
                    disposeSocketAsyncEventArgs(e);
                e.Completed -= onSocketAsyncEventArgsCompleted;
                e.Dispose();
            }

            /// <summary>
            /// Освобождение ресурсов
            /// </summary>
            /// <param name="disposing"></param>
            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    //Net.TcpSocket.TcpSocketState tss;
                    foreach (var e in stackSocketAsyncEventArgs)
                    {
                        DisposeItem(e);
                    }
                    stackSocketAsyncEventArgs.Clear();
                    stackSocketAsyncEventArgs = null;
                    disposeSocketAsyncEventArgs = null;
                    newSocketAsyncEventArgs = null;
                    onSocketAsyncEventArgsCompleted = null;
                }
                base.Dispose(disposing);
            }
        }

        //// This method closes the read socket and gets rid of our user token associated with it
        //private void PoolSocketAsyncEventArgsFree(TcpSocketAsyncEventArgs e)
        //{
        //    //TcpSocketState socketState = readSocket.UserToken as TcpSocketState;
        //    //if (socketState != null)
        //    //{
        //    //    socketState.Dispose();
        //    //    readSocket.UserToken = null;
        //    //}

        //    // Decrement the counter keeping track of the total number of clients connected to the server.
        //    //***Interlocked.Decrement(ref numconnections);

        //    // Put the read socket back in the stack to be used again
        //    poolEventArgs.Push(e);
        //}
    }
}
