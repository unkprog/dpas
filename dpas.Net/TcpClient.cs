using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace dpas.Net
{
    public partial class TcpClient : TcpSocket
    {
        /// <summary>
        /// Асинхронное отключение от сервера
        /// </summary>
        /// <returns>True - в случае успешного отключения сервера</returns>
        public async Task<bool> DisconnectAsync()
        {
            if (await Task.Run(() => this.CheckDisconnect()))
                return await Task.Run(() => this.Disconnect());
            else
                return false;
        }

        private bool CheckDisconnect()
        {
            if (this.State != TcpClentState.Connect)
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
                //DisposePoolHandlers();
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

      

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                OnConnect = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Асинхронное подключение к серверу.
        /// </summary>
        /// <returns>True - в случае успешного завершения</returns>
        public async Task<bool> ConnectAsync()
        {
            if (await Task.Run(() => this.CheckConnect()))
                return await Task.Run(() => this.Connect());
            else
                return false;
        }

        private bool CheckConnect()
        {
            if (!(this.State ==  TcpClentState.Unknown || this.State == TcpClentState.Disconnect))
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
            if (!CreateSocket(this.Settings.Server, this.Settings.Port)) return false;
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
