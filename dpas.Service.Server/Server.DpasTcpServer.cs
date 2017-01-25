using System.Text;
using dpas.Net;
using dpas.Service.Protocol;

namespace dpas.Service
{
    internal class DpasTcpServer: TcpServer
    {
        static byte[] DPAS = Encoding.ASCII.GetBytes("DPAS");//.UTF8.GetBytes("DPAS");
        protected override void OnReceiveHandle(TcpSocketAsyncEventArgs e)
        {
            byte[] data = e.ToArray();

            if (data[0] == DPAS[0] && data[1] == DPAS[1] && data[2] == DPAS[2] && data[3] == DPAS[3])
                HandleDpasProtocol(e, data);
            else
            {
                HandleHttpProtocol(e, data);
                // Для Http закрываем соединение, как только произведем обработку
                CloseProcessReceive(e);
            }
        }


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
            void Handle(TcpSocketAsyncEventArgs e, byte[] data);
        }

        /// <summary>
        /// Поддерживаемые сервером протоколы
        /// </summary>
        IProtocol dpasProtocol, httpProtocol;

        private void HandleDpasProtocol(TcpSocketAsyncEventArgs e, byte[] data)
        {
            if (dpasProtocol == null)
                dpasProtocol = new DpasProtocol() { BufferSize = this.Settings.BufferSize };
            dpasProtocol.Handle(e, data);
        }


        private void HandleHttpProtocol(TcpSocketAsyncEventArgs e, byte[] data)
        {
            if (httpProtocol == null)
                httpProtocol = new HttpProtocol() { BufferSize = Settings.BufferSize };
            httpProtocol.Handle(e, data);
        }

        private void SetupProtocols()
        {
            if (dpasProtocol != null)
                dpasProtocol.BufferSize = Settings.BufferSize;
            if (httpProtocol != null)
                httpProtocol.BufferSize = Settings.BufferSize;
        }
    }
}
