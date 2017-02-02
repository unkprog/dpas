using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace dpas.Net
{
    public partial class TcpClient : TcpSocket
    {
        public TcpClient(ILoggerFactory loggerFactory) : base(loggerFactory)
        {

        }
        /// <summary>
        /// Асинхронное отключение от сервера
        /// </summary>
        /// <returns>True - в случае успешного отключения сервера</returns>
        public async Task<bool> DisconnectAsync()
        {
            if (await Task.Run(() => CheckDisconnect()))
                return await Task.Run(() => Disconnect());
            else
                return false;
        }

        /// <summary>
        /// Проверка при отключении от сервера
        /// </summary>
        /// <returns>True - в случае если подключены к серверу</returns>
        private bool CheckDisconnect()
        {
            if (State != TcpClentState.Connect)
            {
                WriteToLog("Клиент не подключен к серверу...");
                return false;
            }
            SetState(TcpClentState.Disconnecting);
            return true;
        }


        /// <summary>
        /// Отключение от сервера
        /// </summary>
        /// <returns>True - в случае успешного отключения сервера</returns>
        private bool Disconnect()
        {
            try
            {
                DisposeSocket();
                SetState(TcpClentState.Disconnect);
            }
            catch (Exception ex)
            {
                SetException(ex, "TcpClient.Disconnect():");
                return false;
            }
            return true;
        }

      
        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disconnect();
                OnConnect = null;
                OnDisconnect = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Асинхронное подключение к серверу.
        /// </summary>
        /// <returns>True - в случае успешного завершения</returns>
        public async Task<bool> ConnectAsync()
        {
            if (await Task.Run(() => CheckConnect()))
                return await Task.Run(() => Connect());
            else
                return false;
        }

        /// <summary>
        /// Проверка при подключении к серверу
        /// </summary>
        /// <returns>True - в случае если не подключены к серверу</returns>
        private bool CheckConnect()
        {
            if (!(State ==  TcpClentState.Unknown || State == TcpClentState.Disconnect))
            {
                WriteToLog(string.Concat("Клиент находится в статусе ", State, ", подключение невозможно..."));
                return false;
            }
            SetState(TcpClentState.Connecting);
            return true;
        }

        private void InitClient()
        {
            poolEventArgs.loggingTag = "TcpClient.poolEventArgs";
            isLogging = poolEventArgs.isLogging = Settings.IsLogging;
        }


        /// <summary>
        /// Пподключение к серверу.
        /// </summary>
        /// <returns>True - в случае успешного завершения</returns>
        private bool Connect()
        {
            if (!CreateSocket(Settings.Server, Settings.Port)) return false;
            InitClient();
            try
            {
                TcpSocketAsyncEventArgs e = poolEventArgs.Pop();
                e.RemoteEndPoint = endpoint;
                socket.ConnectAsync(e);
            }
            catch (Exception ex)
            {
                SetException(ex, "TcpClient.Connect():");
                return false;
            }
            return true;
        }

    }
}
