using System;
using System.Text;
using System.Collections.Generic;

namespace dpas.Net.Http
{
    public interface IHttpParameters : IDictionary<string, string>
    {

    }
    public class HttpParameters : Dictionary<string, string>, IHttpParameters
    {
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var param in this)
            {
                result.Append(param.Key);
                result.Append(": ");
                result.Append(param.Value);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }
    }
}
