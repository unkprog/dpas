using System;
using dpas.Net;

namespace dpas.Service.Protocol
{
    public class DpasProtocol : Server.IProtocol
    {
        void Server.IProtocol.Handle(TcpSocket.TcpSocketAsyncEventArgs e, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
