namespace dpas.Net.Http.Mvc
{
    public class ControllerInfo
    {
        public string Url { get; private set; }
        public string Prefix { get; private set; }
        public string Controller { get; private set; }
        public string Action { get; private set; }
        public string QueryString { get; private set; }
        public string Path { get; private set; }

        public string Content { get; private set; }

        public string CurrentPage { get; private set; }

        public ControllerInfo(IHttpContext context) : this(context.Request.Url, context.Request.Content)
        {
        }
        public ControllerInfo(string url, string content)
        {
            string curUrl = url;
            Url = curUrl;
            Prefix = string.Empty;
            Controller = string.Empty;
            Action = string.Empty;
            QueryString = string.Empty;
            Content = content;
            if (string.IsNullOrEmpty(curUrl)) return;

            int index = curUrl.IndexOf('/');
            if (index > -1 && curUrl.Length > 1)
            {
                Prefix = curUrl.Substring(0, index + 1);
                curUrl = curUrl.Substring(index + 1);
                index = curUrl.IndexOf('/');
                if (index > -1)
                {
                    Prefix = string.Concat(Prefix, curUrl.Substring(0, index));
                    curUrl = curUrl.Substring(index);
                }
            }

            index = curUrl.LastIndexOf('/');
            if (index > 0)
            {
                Controller = curUrl.Substring(0, index);
                curUrl = curUrl.Substring(index);
            }

            index = curUrl.IndexOf('?');
            if (index > -1)
            {
                Action = curUrl.Substring(0, index);
                QueryString = curUrl.Substring(index);
            }
            else
            {
                if (string.IsNullOrEmpty(Controller))
                    Controller = curUrl;
                else
                    Action = curUrl;
            }

            CurrentPage = string.Concat(Controller, Action);
            Path = string.Concat(Prefix, CurrentPage);
        }


        public override string ToString()
        {
            return Url;
        }
    }
}
