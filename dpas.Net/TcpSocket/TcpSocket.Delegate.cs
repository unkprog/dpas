namespace dpas.Net
{
    public partial class TcpSocket
    {
        //public class SocketEventArgs
        //{
        //    public SocketEventArgs(TcpSocketState state)
        //    {
        //        State = state;
        //    }
        //    public TcpSocketState State { get; private set; }
        //}

        //public class SocketReceiveArgs : SocketEventArgs
        //{
        //    public SocketReceiveArgs(TcpSocketState state) : base(state)
        //    {
        //        Data = state.ToArray();
        //    }
        //    public byte[] Data { get; private set; }

        //}

        public delegate void SocketHandler(object sender, TcpSocketAsyncEventArgs e);
    }
}
