using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace dpas.Net
{
    public partial class TcpServer : TcpSocket
    {
        //private static Mutex mutex = null;
        private ManualResetEvent listenEvent = null;

        /// <summary>
        /// Метод для остановки сервера и освобождения ресурсов
        /// </summary>
        /// <returns>True - в случае успешной остановки сервера</returns>
        public async Task<bool> StopAsync()
        {
            if (await Task.Run(() => CheckStop()))
                return await Task.Run(() => Stop());
            else
                return false;
        }

        private bool CheckStop()
        {
            if (State != TcpServer.ServerState.Started)
            {
                WriteToLog("Сервер не запущен...");
                return false;
            }
            SetState(ServerState.Stopping);
            return true;
        }

        private bool Stop()
        {
            base.DisposeSocket(false);
            DisposeListenEvent();
            SetState(ServerState.Stopped);
            return true;
        }

        private void DisposeListenEvent()
        {
            if (listenEvent != null)
            {
                listenEvent.Dispose();
                listenEvent = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeListenEvent();
                OnStart = null;
            }
            base.Dispose(disposing);
        }

        private void InitServer()
        {
            poolEventArgs.loggingTag = "TcpServer.poolEventArgs";
            isLogging = poolEventArgs.isLogging = Settings.IsLogging;
            //// Set up our mutex and semaphore
            listenEvent = new ManualResetEvent(false);
        }
        protected override PoolSocketAsyncEventArgs CreatePoolSocketAsyncEventArgs()
        {
            return new PoolSocketAsyncEventArgs(this, Settings.MaxConnections, OnSocketAsyncEventArgsCompleted
                , null //() => { return new TcpSocketAsyncEventArgs(this.Settings.BufferSize); }
                , null //(e)=> { }
                );
        }

        public async Task<bool> StartAsync()
        {
            if (await Task.Run(() => CheckStart()))
                return await Task.Run(() => Start());
            else
                return false;
        }

        private bool CheckStart()
        {
            if (!(State == TcpServer.ServerState.Unknown || State == ServerState.Stopped))
            {
                WriteToLog(string.Concat("Сервер находится в статусе ", State, ", запуск невозможен..."));
                return false;
            }
            SetState(ServerState.Starting);
            return true;
        }

        

        private bool Start()
        {
            if (!CreateSocket(string.Empty, Settings.Port)) return false;
            InitServer();
            try
            {
                // Биндим слушателя к локальному IP
                socket.Bind(endpoint);
                // Запускаем прослушиватель и ждем подключений
                socket.Listen(Settings.MaxConnections);
                SetState(ServerState.Started);
                OnStartHandle();
                LoopProcessAccept();
                return true;
            }
            catch (Exception ex)
            {
                SetException(ex, "TcpServer.Start():");
                return false;
            }
        }

       

        /// <summary>
        /// Метод реализует цикл асинхронной обработки входящих подключений
        /// </summary>
        private void LoopProcessAccept()
        {
            while (IsStarted)
            {
                listenEvent.Reset();

                TcpSocketAsyncEventArgs e = poolEventArgs.Pop();
                if (!socket.AcceptAsync(e))
                    ProcessAccept(e);

                listenEvent.WaitOne();
            }
        }
    }
}
