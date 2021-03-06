﻿using System.Net.Sockets;

namespace dpas.Net
{
    public partial class TcpSocket
    {

        // Обработчик событий сокета
        /// <summary>
        /// Асинхронный обработчик событий сокета
        /// </summary>
        /// <param name="sender">Объект, для которого произошло событие</param>
        /// <param name="e">Параметр с текущим состоянием сокета</param>
        protected void OnSocketAsyncEventArgsCompleted(object sender, SocketAsyncEventArgs e)
        {
            TcpSocketAsyncEventArgs ee = (TcpSocketAsyncEventArgs)e;

//#if DEBUG
//            if (isLogging)
//                WriteToLog("OnSocketAsyncEventArgsCompleted(Tobject sender, SocketAsyncEventArgs e): LastOperation = " + e.LastOperation + @", SocketError = " + e.SocketError);
//#endif
            if (e.SocketError != SocketError.Success)
            {
                SetError(string.Concat("LastOperation =", e.LastOperation, ", SocketError=", e.SocketError), "TcpSocket.OnSocketAsyncEventArgsCompleted(object sender, SocketAsyncEventArgs e):");
                ProcessError(ee);
                return;
            }
            // Определяем, какой тип операции был завершен и вызываем соответствующий обработчик
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Accept    : ProcessAccept(ee); break;
                case SocketAsyncOperation.Connect   : ProcessConnect(ee); break;
                case SocketAsyncOperation.Disconnect: ProcessDisconnect(ee); break;
                case SocketAsyncOperation.Receive   : ProcessReceive(ee); break;
                case SocketAsyncOperation.Send      : ProcessSend(ee); break;
                default:
                    ProcessOther(ee); break;
            }
        }

        /// <summary>
        /// Обработка асинхронного события при возникновении ошибки
        /// </summary>
        /// <param name="e">Параметр с текущим состоянием сокета</param>
        protected virtual void ProcessError(TcpSocketAsyncEventArgs e)
        {
            // В случае ошибки всегда возвращаем в пул объект события
            CloseConnection(e);
            poolEventArgs.Push(e);
#if DEBUG
            if (isLogging)
                WriteToLog("ProcessError(TcpSocketAsyncEventArgs e): CountPool = " + poolEventArgs.CountEventsLock);
#endif
        }

        /// <summary>
        /// Обработка асинхронного события при получении нового подключения
        /// </summary>
        /// <param name="e">Параметр с текущим состоянием сокета</param>
        protected virtual void ProcessAccept(TcpSocketAsyncEventArgs e) { }

        /// <summary>
        /// Обработка асинхронного события при завершении операции подключения к серверу
        /// </summary>
        /// <param name="e">Параметр с текущим состоянием сокета</param>
        protected virtual void ProcessConnect(TcpSocketAsyncEventArgs e) { }

        /// <summary>
        /// Обработка асинхронного события при завершении операции отключения от сервера
        /// </summary>
        /// <param name="e">Параметр с текущим состоянием сокета</param>
        protected virtual void ProcessDisconnect(TcpSocketAsyncEventArgs e) { }

        public void CloseConnection(TcpSocketAsyncEventArgs e)
        {
#if DEBUG
            if (isLogging)
                WriteToLog("ProcessReceive: Connection closed.");
#endif
            e.CloseSocket();
            e.Socket.Dispose();
            poolEventArgs.Push(e);
        }

        /// <summary>
        /// Обработка события чтения данных из сокета
        /// </summary>
        /// <param name="e">Параметр с текущим состоянием сокета</param>
        protected virtual void ProcessReceive(TcpSocketAsyncEventArgs e)
        {
            // Если количество переданных байтов 0 или принимающий сокет удален, то закроем соединение
            if (e.BytesTransferred == 0 || socket == null)
            {
                if (isLogging)
                    WriteToLog(string.Concat("ProcessReceive -> e.BytesTransferred == 0 || socket == null"));
                CloseConnection(e);
                return;
            }

            e.Read();

#if DEBUG
            if (isLogging)
                WriteToLog(string.Concat("ProcessReceive Num=", e.Num, ", Available=", e.Socket.Available, ", BytesTransferred=", e.BytesTransferred, ", Offset=", e.Offset, ", Count=", e.Count));
#endif

            // Прочитаны все данные, можем их теперь обработать
            if (e.Socket.Available == 0)
            {
                if (isLogging)
                {
                    WriteToLog(string.Concat("ProcessReceive END:  Num=", e.Num, ", Available=", e.Socket.Available, ", BytesTransferred=", e.BytesTransferred, ", Offset=", e.Offset, ", Count=", e.Count));
                }
                //e.IsClear = true;
                OnReceiveHandle(e);
                //if (e.IsClear)
                e.Clear();
            }
            // и продолжаем читать дальше
            if (!e.IsClosed)
                if (!e.Socket.ReceiveAsync(e))
                    ProcessReceive(e);
        }

        /// <summary>
        /// Обработка асинхронного события при завершении операции отправки данных серверу
        /// </summary>
        /// <param name="e">Параметр с текущим состоянием сокета</param>
        protected virtual void ProcessSend(TcpSocketAsyncEventArgs e)
        {
#if DEBUG
            if (isLogging)
                WriteToLog(string.Concat("ProcessSend: Send ", e.BytesTransferred, " bytes"));
#endif
            OnSendHandle(e);
        }

        /// <summary>
        /// Обработка асинхронного события, для которого не назначен обработчик
        /// </summary>
        /// <param name="e">Параметр с текущим состоянием сокета</param>
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
            OnSend?.Invoke(this, e);
        }

        protected virtual void OnReceiveHandle(TcpSocketAsyncEventArgs e)
        {
            OnReceive?.Invoke(this, e);
        }
    }
}
