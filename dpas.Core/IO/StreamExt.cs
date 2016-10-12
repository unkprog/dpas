using System.IO;

namespace dpas.Core.IO
{
    public static class StreamExt
    {
        /// <summary>
        /// Чтение файла
        /// </summary>
        /// <param name="stream">Поток, в который пишем</param>
        /// <param name="path">Путь, по которому расположен файл</param>
        public static void ReadFromFile(this Stream stream, string path, int bufferSize = 4096)
        {
            using (FileStream fileStream = File.OpenRead(path))
            {
                byte[] buffer = new byte[bufferSize];
                int readBytesCount = 0;
                while ((readBytesCount = fileStream.Read(buffer, 0, bufferSize)) > 0)
                    stream.Write(buffer, 0, readBytesCount);
            }
        }
    }
}
