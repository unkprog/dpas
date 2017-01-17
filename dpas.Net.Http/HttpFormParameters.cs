using System.Collections.Generic;
using System.Text;

namespace dpas.Net.Http
{
    public interface IHttpFormParameters : IDictionary<string, string>
    {

    }
    public class HttpFormParameters : Dictionary<string, string>, IHttpFormParameters
    {
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            bool isNotOneParam = false;
            foreach (var param in this)
            {
                if(isNotOneParam)
                    result.Append('&');
                result.Append(param.Key);
                result.Append('=');
                result.Append(param.Value);
                isNotOneParam = true;
            }
            return result.ToString();
        }
    }
}
