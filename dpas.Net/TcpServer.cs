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
            if (await Task.Run(() => this.CheckStop()))
                return await Task.Run(() => this.Stop());
            else
                return false;
        }

        private bool CheckStop()
        {
            if (this.State != TcpServer.TcpServerState.Started)
            {
                WriteToLog("Сервер не запущен...");
                return false;
            }
            SetState(TcpServerState.Stopping);
            return true;
        }

        private bool Stop()
        {
            base.DisposeSocket(false);
            DisposeListenEvent();
            SetState(TcpServerState.Stopped);
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
            return new PoolSocketAsyncEventArgs(this, this.Settings.MaxConnections, this.OnSocketAsyncEventArgsCompleted
                , null //() => { return new TcpSocketAsyncEventArgs(this.Settings.BufferSize); }
                , null //(e)=> { }
                );
        }

        public async Task<bool> StartAsync()
        {
            if (await Task.Run(() => this.CheckStart()))
                return await Task.Run(() => this.Start());
            else
                return false;
        }

        private bool CheckStart()
        {
            if (!(this.State == TcpServer.TcpServerState.Unknown || this.State == TcpServerState.Stopped))
            {
                WriteToLog(string.Concat("Сервер находится в статусе ", State, ", запуск невозможен..."));
                return false;
            }
            SetState(TcpServerState.Starting);
            return true;
        }

        

        private bool Start()
        {
            if (!CreateSocket(string.Empty, this.Settings.Port)) return false;
            InitServer();
            try
            {
                // Биндим слушателя к локальному IP
                socket.Bind(endpoint);
                // Запускаем прослушиватель и ждем подключений
                socket.Listen(this.Settings.MaxConnections);
                SetState(TcpServerState.Started);
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
            while (this.IsStarted)
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
