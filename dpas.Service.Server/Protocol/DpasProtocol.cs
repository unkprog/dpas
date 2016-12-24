using System;
using dpas.Net;
using static dpas.Service.DpasTcpServer;

namespace dpas.Service.Protocol
{
    public class DpasProtocol : IProtocol
    {
        public DpasProtocol()
        {
            BufferSize = 1024;
        }
        public int BufferSize { get; set; }
        void IProtocol.Handle(TcpSocket.TcpSocketAsyncEventArgs e, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
