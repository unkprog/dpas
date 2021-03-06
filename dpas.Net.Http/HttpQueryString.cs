﻿using System.Collections.Generic;
using System.Text;

namespace dpas.Net.Http
{
    public interface IHttpQueryString : IDictionary<string, string>
    {

    }
    public class HttpQueryString : Dictionary<string, string>, IHttpQueryString
    {
        public override string ToString()
        {
            if (this.Count == 0) return string.Empty;
            StringBuilder result = new StringBuilder();
            result.Append('?');
            bool isNotOneParam = false;
            foreach (var param in this)
            {
                if (isNotOneParam)
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
