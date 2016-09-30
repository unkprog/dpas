using System;
using dpas.Net;

namespace dpas.Service.Protocol
{
    public class DpasProtocol : Server.IProtocol
    {
        public DpasProtocol()
        {
            BufferSize = 1024;
        }
        public int BufferSize { get; set; }
        void Server.IProtocol.Handle(TcpSocket.TcpSocketAsyncEventArgs e, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
