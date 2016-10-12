namespace dpas.Net.Http
{
    public class HttpFile
    {
        public HttpFile()
        {

        }

        public HttpFile(string file)
        {
            SetFile(file);
        }

        public void SetFile(string file)
        {
            if (!string.IsNullOrEmpty(file))
            {
                int indexExt = file.LastIndexOf('.');
                if (indexExt > -1)
                {
                    FileName = file.Substring(0, indexExt);
                    FileExt = indexExt < file.Length ? file.Substring(indexExt, file.Length - indexExt) : string.Empty;
                }
            }
        }
        public string FileName { get; private set; }
        public string FileExt  { get; private set; }

        public override string ToString()
        {
            return string.Concat(FileName, FileExt);
        }
    }
}
