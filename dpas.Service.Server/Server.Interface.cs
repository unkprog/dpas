using System;
using dpas.Net;

namespace dpas.Service
{
    /// <summary>
    /// Интерфейс сервера
    /// </summary>
    public interface IServer: IDisposable
    {
        /// <summary>
        /// Состояние сервера
        /// </summary>
        TcpServer.ServerState State { get; }

        /// <summary>
        /// Настройки сервера
        /// </summary>
        TcpServer.ServerSettings Settings { get; }

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
        TcpServer.ServerState IServer.State { get { return server.State; } }

        TcpServer.ServerSettings IServer.Settings { get { return server.Settings; } }

        void IServer.Start()
        {
            if (server == null)
                server = new TcpServer();
            CreateDirectories();
            server.Settings.Read(settingsFile);
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