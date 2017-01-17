namespace dpas.Net.Http
{
    public partial class HttpParser
    {
        private const byte _space   = (byte)' ';
        private const byte _enter   = 13;
        private const byte _newline = 10;
        private const byte _paramDelimiter = (byte)':';


        private const int HttpHeader_Method = 0;
        private const int HttpHeader_Path = 1;
        private const int HttpHeader_Protocol = 2;
    }
}
