namespace dpas.Net
{
    public partial class TcpClient
    {
        public enum TcpClentState
        {
            Unknown = 0,
            Connecting = 1,
            Connect = 2,
            Disconnecting = 3,
            Disconnect = 4
        }

        /// <summary>
        /// Флаг индикации, что сокет подключен
        /// </summary>
        public bool IsConnected { get { return State == TcpClentState.Connect; } }

        /// <summary>
        /// Текущий статус сокета
        /// </summary>
        public TcpClentState State { get; private set; } = TcpClentState.Unknown;

        /// <summary>
        /// Установка статуса сокета
        /// </summary>
        /// <param name="state">Устанавливаемый статус</param>
        private void SetState(TcpClentState state)
        {
            State = state;
            if (this.Settings.IsLogging)
                WriteToLog(string.Concat("State=", State));
        }

        /// <summary>
        /// Процедура записи в лог информационного сообщения
        /// </summary>
        /// <param name="data">Сообщение</param>
        public override void WriteToLog(string data)
        {
            string logData = socket == null ? string.Empty : string.Concat("(", socket.AddressFamily, ", ", endpoint, ")");
            logData = string.Concat("TcpClient ", logData, ": ", data);
            base.WriteToLog(logData);
        }
    }
}
