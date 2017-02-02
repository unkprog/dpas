using System.Text;
using dpas.Net;
using dpas.Service.Protocol;
using Microsoft.Extensions.Logging;
using System;

namespace dpas.Service
{
    internal class DpasTcpServer : TcpServer
    {
        public DpasTcpServer(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

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
                //CloseProcessReceive(e);
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
            DumpData(data);
            if (httpProtocol == null)
                httpProtocol = new HttpProtocol(this) { BufferSize = Settings.BufferSize };
            httpProtocol.Handle(e, data);
        }

        private void DumpData(byte[] data)
        {
            if (_logger != null)
            {
                string inputData = Encoding.UTF8.GetString(data);
                WriteToLog(string.Concat(Environment.NewLine, inputData));

                int i, icount = data.Length, divider = 20;

                int last = icount % divider, rows = (icount - last) / divider;
                StringBuilder sb = new StringBuilder(string.Concat(Environment.NewLine, "DumpData request: ", icount, Environment.NewLine));
                string row;
                for (int j = 0; j < rows; j++)
                {
                    row = string.Empty;
                    for (i = 0; i < divider; i++)
                    {
                        row = string.Format("{0}{1:X2} ", row, data[j * 10 + i]);
                    }
                    sb.Append(string.Concat(row, Environment.NewLine));
                }
                row = string.Empty;
                for (i = 0; i < last; i++)
                {
                    row = string.Format("{0}{1:X2} ", row, data[rows * divider + i]);
                }
                sb.Append(string.Concat(row, Environment.NewLine));

                WriteToLog(sb.ToString());
            }
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
