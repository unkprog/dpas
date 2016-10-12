namespace dpas.Net.Http.Mvc
{
    public class ControllerInfo
    {
        public string Url { get; private set; }
        public string Controller { get; private set; }
        public string Action { get; private set; }
        public string QueryString { get; private set; }
        public string Path { get; private set; }
        public ControllerInfo(string url)
        {
            Url = string.Concat(url);
            Controller = string.Empty;
            Action = string.Empty;
            QueryString = string.Empty;

            if (string.IsNullOrEmpty(Url)) return;

            int index = Url.LastIndexOf('/');
            if (index > -1)
            {
                Controller = Url.Substring(0, index);
                Action = Url.Substring(index);
                index = Action.IndexOf('?');
                if (index > -1)
                {
                    QueryString = Action.Substring(index);
                    Action = Action.Substring(0, index);
                }
            }
            else
                Controller = Url;

            Path = string.Concat(Controller, Action);
        }


        public override string ToString()
        {
            return Url;
        }
    }
}
