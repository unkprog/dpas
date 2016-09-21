using System;
using System.Collections.Generic;
using dpas.Core;
using System.Net.Sockets;

namespace dpas.Net
{
    public partial class TcpSocket
    {


        //////    /// <summary>
        //////    /// class OSUserToken : IDisposable
        //////    /// This class represents the instantiated read socket on the server side.
        //////    /// It is instantiated when a server listener socket accepts a connection.
        //////    /// </summary>
        //////    sealed class SocketState : Disposable
        //////{
        //////    // This is a ref copy of the socket that owns this token
        //////    private Socket ownersocket;

        //////    //// this stringbuilder is used to accumulate data off of the readsocket
        //////    //private StringBuilder stringbuilder;

        //////    // This stores the total bytes accumulated so far in the stringbuilder
        //////    private Int32 totalbytecount;

        //////    // We are holding an exception string in here, but not doing anything with it right now.
        //////    public String LastError = "";

        //////    // The read socket that creates this object sends a copy of its "parent" accept socket in as a reference
        //////    // We also take in a max buffer size for the data to be read off of the read socket
        //////    public SocketState(Socket readSocket, Int32 bufferSize)
        //////    {
        //////        ownersocket = readSocket;
        //////        //stringbuilder = new StringBuilder(bufferSize);
        //////    }

        //////    // This allows us to refer to the socket that created this token's read socket
        //////    public Socket OwnerSocket
        //////    {
        //////        get
        //////        {
        //////            return ownersocket;
        //////        }
        //////    }


        //////    // Do something with the received data, then reset the token for use by another connection.
        //////    // This is called when all of the data have been received for a read socket.
        //////    public void ProcessData(SocketAsyncEventArgs args)
        //////    {
        //////        //// Get the last message received from the client, which has been stored in the stringbuilder.
        //////        //String received = stringbuilder.ToString();

        //////        ////TODO Use message received to perform a specific operation.
        //////        //Console.WriteLine("Received: \"{0}\". The server has read {1} bytes.", received, received.Length);

        //////        //TODO: Load up a send buffer to send an ack back to the calling client
        //////        //Byte[] sendBuffer = Encoding.ASCII.GetBytes(received);
        //////        //args.SetBuffer(sendBuffer, 0, sendBuffer.Length);

        //////        // Clear StringBuffer, so it can receive more data from the client.
        //////        //stringbuilder.Length = 0;
        //////        totalbytecount = 0;
        //////    }


        //////    // This method gets the data out of the read socket and adds it to the accumulator string builder
        //////    public bool ReadSocketData(SocketAsyncEventArgs readSocket)
        //////    {
        //////        Int32 bytecount = readSocket.BytesTransferred;

        //////        //if ((totalbytecount + bytecount) > stringbuilder.Capacity)
        //////        //{
        //////        //    LastError = "Receive Buffer cannot hold the entire message for this connection.";
        //////        //    return false;
        //////        //}
        //////        //else
        //////        //{
        //////        //    stringbuilder.Append(Encoding.ASCII.GetString(readSocket.Buffer, readSocket.Offset, bytecount));
        //////            totalbytecount += bytecount;
        //////            return true;
        //////        //}
        //////    }

        //////    // This is a standard IDisposable method
        //////    // In this case, disposing of this token closes the accept socket
        //////    protected override void Dispose(bool disposing)
        //////    {

        //////        if (disposing)
        //////        {
        //////            try
        //////            {
        //////                ownersocket.Shutdown(SocketShutdown.Both);
        //////            }
        //////            catch
        //////            {
        //////                //Nothing to do here, connection is closed already
        //////            }
        //////            finally
        //////            {
        //////                ownersocket.Dispose(); //.Close();
        //////            }
        //////            ownersocket = null;
        //////        }
        //////        base.Dispose(disposing);
        //////    }
        //////}

        //// State object for socket data
        public sealed class TcpSocketState : Disposable
        {
            public class SocketData
            {
                public int bytesRead;
                public byte[] buffer;
            }

            public TcpSocketState(Socket socket)
            {
                Socket = socket;
                Data = new List<SocketData>();
            }

            public Socket Socket { get; internal set; }
            public List<SocketData> Data { get; internal set; }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    Data.Clear();
                    Data = null;
                    Socket = null;
                }
                base.Dispose(disposing);
            }

            public void Clear()
            {
                Data.Clear();
            }

            public bool ReadSocketData(SocketAsyncEventArgs readSocket)
            {
                int bytecount = readSocket.BytesTransferred;
                SocketData buffer = new SocketData() { buffer = new byte[bytecount] };
                Data.Add(buffer);
                Array.Copy(readSocket.Buffer, readSocket.Offset, buffer.buffer, 0, bytecount);
                return true;
            }

            public byte[] ToArray()
            {
                byte[] result = null;
                int countRead = 0;
                int i, icount = Data.Count;
                for (i = 0; i < icount; i++)
                    countRead += Data[i].bytesRead;
                result = new byte[countRead];
                countRead = 0;
                SocketData bufferInfo;
                for (i = 0; i < icount; i++)
                {
                    bufferInfo = Data[i];
                    Array.Copy(bufferInfo.buffer, 0, result, countRead, bufferInfo.bytesRead);
                    countRead += bufferInfo.bytesRead;
                }
                return result;
            }
        }

        //public class TcpSocketStateMemoryStream : TcpSocketState
        //{
        //    public TcpSocketStateMemoryStream(TcpSocket socket) : base(socket)
        //    {
        //        Stream = new MemoryStream();
        //    }

        //    public TcpSocketStateMemoryStream(TcpSocket socket, int bufferSize) : this(socket)
        //    {
        //        Buffer = new byte[bufferSize];
        //    }

        //    public MemoryStream Stream { get; private set; }
        //    public byte[] Buffer = null;

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            Buffer = null;
        //            if (Stream != null) Stream.Dispose();
        //            Stream = null;
        //        }
        //        base.Dispose(disposing);
        //    }
        //}

        //// State object for reading socket data
        //public class TcpSocketStateReader : TcpSocketStateMemoryStream
        //{
        //    public TcpSocketStateReader(TcpSocket socket)
        //        : base(socket)
        //    {
        //        Reader = new BinaryReader(Stream);
        //    }
        //    public TcpSocketStateReader(TcpSocket socket, int bufferSize)
        //        : base(socket, bufferSize)
        //    {
        //        Reader = new BinaryReader(Stream);
        //    }

        //    public BinaryReader Reader { get; private set; }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            if (Reader != null) Reader.Dispose();
        //            Reader = null;
        //        }
        //        base.Dispose(disposing);
        //    }
        //}

        //// State object for reading writing data 
        //public class TcpSocketStateWriter : TcpSocketStateMemoryStream
        //{
        //    public TcpSocketStateWriter(TcpSocket socket)
        //        : base(socket)
        //    {
        //        Writer = new BinaryWriter(Stream);
        //    }
        //    public TcpSocketStateWriter(TcpSocket socket, int bufferSize)
        //        : base(socket, bufferSize)
        //    {
        //        Writer = new BinaryWriter(Stream);
        //    }

        //    public BinaryWriter Writer { get; private set; }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            if (Writer != null) Writer.Dispose();
        //            Writer = null;
        //        }
        //        base.Dispose(disposing);
        //    }
        //}
    }
}