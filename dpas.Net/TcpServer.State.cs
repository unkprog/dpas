﻿namespace dpas.Net
{
    public partial class TcpServer
    {
        public enum TcpServerState
        {
            Unknown = 0,
            Starting = 1,
            Started = 2,
            Stopping = 3,
            Stopped = 4
        }

        /// <summary>
        /// Флаг индикации, что сервер запущен
        /// </summary>
        public bool IsStarted {  get { return State == TcpServerState.Started; } }

        /// <summary>
        /// Текущий статус сервера
        /// </summary>
        public TcpServerState State { get; private set; } = TcpServerState.Unknown;


        private void SetState(TcpServerState state)
        {
            State = state;
            if (this.Settings.IsLogging)
                WriteToLog(string.Concat("State=", State));
        }

        public override void WriteToLog(string data)
        {
            string logData = socket == null ? string.Empty : string.Concat("(", socket.AddressFamily, ", ", endpoint, ")");
            logData = string.Concat("TcpServer ", logData, ": ", data);
            base.WriteToLog(logData);
        }
    }
}
