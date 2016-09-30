
using dpas.Net;
using dpas.Service.Protocol;

namespace dpas.Service
{
    public partial class Server
    {
        public interface IProtocol
        {
            /// <summary>
            /// Размер буфера 
            /// </summary>
            int BufferSize { get; set; }

            /// <summary>
            /// Обработка данных сокета
            /// </summary>
            /// <param name="e">Аргумент события сокета</param>
            /// <param name="data">Данные</param>
            void Handle(TcpSocket.TcpSocketAsyncEventArgs e, byte[] data);
        }

        /// <summary>
        /// Поддерживаемые сервером протоколы
        /// </summary>
        IProtocol dpasProtocol, httpProtocol;

        private void HandleDpasProtocol(TcpSocket.TcpSocketAsyncEventArgs e, byte[] data)
        {
            if (dpasProtocol == null)
                dpasProtocol = new DpasProtocol() { BufferSize = server.Settings.BufferSize };
            dpasProtocol.Handle(e, data);
        }


        private void HandleHttpProtocol(TcpSocket.TcpSocketAsyncEventArgs e, byte[] data)
        {
            if (httpProtocol == null)
                httpProtocol = new HttpProtocol() { BufferSize = server.Settings.BufferSize };
            httpProtocol.Handle(e, data);
        }

        private void SetupProtocols()
        {
            if (dpasProtocol != null)
                dpasProtocol.BufferSize = server.Settings.BufferSize;
            if (httpProtocol != null)
                httpProtocol.BufferSize = server.Settings.BufferSize;
        }
    }
}
