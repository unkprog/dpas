using System;
using System.Text;
using System.Collections.Generic;

namespace dpas.Net.Http
{
    public interface IHttpCookies : IDictionary<string, string>
    {

    }
    public class HttpCookies : Dictionary<string, string>, IHttpCookies
    {
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            bool isNotOne = false;
            result.Append(HttpHeader.SetCookie);
            result.Append(": ");
            foreach (var param in this)
            {
                if(isNotOne)
                    result.Append("; ");
                result.Append(param.Key);
                result.Append("=");
                result.Append(param.Value);
               
            }
            return result.ToString();
        }
    }
   
}
