using System.Net.Sockets;

namespace dpas.Net
{
    public partial class TcpClient
    {
        public event SocketHandler OnConnect;
        public event SocketHandler OnDisconnect;

        /// <summary>
        /// Обработка асинхронного события при возникновении ошибки
        /// </summary>
        /// <param name="e">Параметр с текущим состоянием сокета</param>
        protected override void ProcessError(TcpSocketAsyncEventArgs e)
        {
#if DEBUG
            if (isLogging)
                WriteToLog("ProcessError");
#endif
            if (e.LastOperation == SocketAsyncOperation.Connect)
                Disconnect();
            base.ProcessError(e);
        }

        /// <summary>
        /// Обработка асинхронного события, для которого не назначен обработчик
        /// </summary>
        /// <param name="e">Параметр с текущим состоянием сокета</param>
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
        /// Обработка асинхронного события при завершении операции подключения к серверу
        /// </summary>
        /// <param name="e">Параметр с текущим состоянием сокета</param>
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
        /// Обработка асинхронного события при завершении операции отключения от сервера
        /// </summary>
        /// <param name="e">Параметр с текущим состоянием сокета</param>
        protected override void ProcessDisconnect(TcpSocketAsyncEventArgs e)
        {
#if DEBUG
            if (isLogging)
                WriteToLog("ProcessDisconnect");
#endif
            base.ProcessDisconnect(e);

            if (OnDisconnect != null)
                OnDisconnect(this, e);

            poolEventArgs.Push(e);
        }

        // Используется только для отладки
        //        protected override void ProcessReceive(TcpSocketAsyncEventArgs e)
        //        {
        //#if DEBUG
        //            if (isLogging)
        //                WriteToLog("ProcessReceive");
        //#endif
        //            base.ProcessReceive(e);
        //        }

        /// <summary>
        /// Обработка события при завершении асинхронной операции отправки данных серверу
        /// </summary>
        /// <param name="e">Параметр с текущим состоянием сокета</param>
        protected override void ProcessSend(TcpSocketAsyncEventArgs e)
        {
            base.ProcessSend(e);
            // Стартуем чтение данных от сервера
            e.Socket.ReceiveAsync(e);
        }
    }
}
