
using dpas.Net;
using dpas.Service.Protocol;

namespace dpas.Service
{
    public partial class Server
    {
        public interface IProtocol
        {
            void Handle(TcpSocket.TcpSocketAsyncEventArgs e, byte[] data);
        }

        /// <summary>
        /// Поддерживаемые сервером протоколы
        /// </summary>
        IProtocol dpasProtocol, httpProtocol;

        private void HandleDpasProtocol(TcpSocket.TcpSocketAsyncEventArgs e, byte[] data)
        {
            if (dpasProtocol == null)
                dpasProtocol = new DpasProtocol();
            dpasProtocol.Handle(e, data);
        }


        private void HandleHttpProtocol(TcpSocket.TcpSocketAsyncEventArgs e, byte[] data)
        {
            if (httpProtocol == null)
                httpProtocol = new HttpProtocol();
            httpProtocol.Handle(e, data);
        }
    }
}
