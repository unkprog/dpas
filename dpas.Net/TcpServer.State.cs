namespace dpas.Net
{
    public partial class TcpServer
    {
        public enum ServerState
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
        public bool IsStarted {  get { return State == ServerState.Started; } }

        /// <summary>
        /// Текущий статус сервера
        /// </summary>
        public ServerState State { get; private set; } = ServerState.Unknown;


        private void SetState(ServerState state)
        {
            State = state;
            if (Settings.IsLogging)
                WriteToLog(string.Concat("State=", State));
        }

        public void WriteToLog(string data)
        {
            string logData = socket == null ? string.Empty : string.Concat("(", socket.AddressFamily, ", ", endpoint, ")");
            logData = string.Concat("TcpServer ", logData, ": ", data);
            base.WriteToLog(logData);
        }
    }
}
