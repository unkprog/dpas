namespace dpas.Net
{
    public partial class TcpSocket
    {
        public delegate void SocketHandler(object sender, TcpSocketAsyncEventArgs e);
    }
}
