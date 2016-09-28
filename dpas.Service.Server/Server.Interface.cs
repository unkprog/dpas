using dpas.Net;

namespace dpas.Service
{
    /// <summary>
    /// Интерфейс сервера
    /// </summary>
    interface IServer
    {
        /// <summary>
        /// Запустить сервер
        /// </summary>
        void Start();

        /// <summary>
        /// Остановить сервер
        /// </summary>
        void Stop();
    }


    public partial class Server
    {
        void IServer.Start()
        {
            if (server == null)
                server = new TcpServer();
            var startTask = server.StartAsync();
        }

        void IServer.Stop()
        {
            if (server != null)
            {
                var stopTask = server.StopAsync();
            }
        }
    }
}