using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using dpas.Core;

namespace dpas.Net
{
    public partial class TcpSocket : Disposable
    {
        protected Socket socket;
        protected IPEndPoint endpoint;
        protected PoolSocketAsyncEventArgs poolEventArgs;

        protected bool isLogging = false;
        protected bool exceptionThrown = false;
        protected string lastError = "";

        protected readonly ILogger _logger;

        public TcpSocket(ILoggerFactory loggerFactory)
        {
            if (loggerFactory != null)
                _logger = loggerFactory.CreateLogger<TcpSocket>();
        }

        public string GetLastError() { return lastError; }

        protected void SetException(System.Exception ex, string methodName = "")
        {
            SetError(ex.ToString(), methodName);
        }

        protected virtual void SetError(string errorMessage, string methodName = "")
        {
            exceptionThrown = true;
            lastError = string.Concat(string.IsNullOrEmpty(methodName) ? string.Empty : methodName + ": ", errorMessage);
            WriteToLog(lastError, true);
        }

        public void WriteToLog(string data, bool isError = false)
        {
            if (_logger != null)
                if (isError)
                    _logger.LogError(data);
            //    else
            //        _logger.LogInformation(data);
        }

        // An IPEndPoint contains all of the information about a server or client
        // machine that a socket needs.  Here we create one from information
        // that we send in as parameters
        private bool CreateIPEndPoint(string server, int port)
        {
            endpoint = null;
            try
            {
                // We get the IP address and stuff from DNS (Domain Name Services)
                // I think you can also pass in an IP address, but I would not because
                // that would not be extensible to IPV6 later
                Task<IPHostEntry> hostInfo = Dns.GetHostEntryAsync(server);
                IPAddress serverAddress = hostInfo.Result.AddressList[0];
                endpoint = new IPEndPoint(serverAddress, port);
            }
            catch (System.Exception ex)
            {
                SetException(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Создание сокета по указанному IP адресу и порту
        /// </summary>
        /// <param name="server">IP адрес</param>
        /// <param name="port">Порт</param>
        /// <returns>True - в случае успеха</returns>
        protected bool CreateSocket(string server, int port)
        {
            exceptionThrown = false;
            string _server = string.IsNullOrEmpty(server) || server == "." ? "localhost" : server;
            if (!CreateIPEndPoint(_server, port)) return false;

            try
            {
                socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                poolEventArgs = CreatePoolSocketAsyncEventArgs();
            }
            catch (System.Exception ex)
            {
                SetException(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Создание пула объектов аргументов для асинхронного сокета
        /// </summary>
        /// <returns>Пул объектов аргументов для асинхронного сокета</returns>
        protected virtual PoolSocketAsyncEventArgs CreatePoolSocketAsyncEventArgs()
        {
            return new PoolSocketAsyncEventArgs(this, 10, OnSocketAsyncEventArgsCompleted, null, null);
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        protected void DisposeSocket(bool shutdown = true)
        {
            if (socket != null)
            {
                if (shutdown)
                    socket.Shutdown(SocketShutdown.Both);
                socket.Dispose();
                socket = null;
            }
            endpoint = null;
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeEvents();
                DisposeSocket();
                if (poolEventArgs != null)
                {
                    poolEventArgs.Dispose();
                    poolEventArgs = null;
                }
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// Асинхронная отправка данных
        /// </summary>
        /// <returns>True - в случае успешной отправки данных</returns>
        public async Task<bool> SendAsync(byte[] data, Socket socketTo = null)
        {
            return await Task.Run(() => Send(data, socketTo == null ? socket : socketTo));
        }


        /// <summary>
        /// Метод для асинхронной отправки данных сокету
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="socketTo">Сокет, которому отправляются данные</param>
        /// <returns>True - в случае успешной отправки</returns>
        protected bool Send(byte[] data, Socket socketTo)
        {
            try
            {
                if (!socketTo.Connected) return false;
                TcpSocketAsyncEventArgs e = poolEventArgs.Pop();
                e.AcceptSocket = socketTo;
                if (e.Buffer == null)
                    e.SetBuffer(data, 0, data.Length);
#if DEBUG
                if (isLogging)
                    WriteToLog(string.Concat("Sending ", data.Length, " bytes"));
#endif
                if (!e.Socket.SendAsync(e))
                    ProcessSend(e);
            }
            catch (System.Exception ex)
            {
                SetException(ex, "TcpSocket.Send(byte[] data):");
                return false;
            }
            return true;
        }
    }
}
