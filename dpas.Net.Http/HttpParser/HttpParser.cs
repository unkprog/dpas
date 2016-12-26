using System;
using System.Text;

namespace dpas.Net.Http
{
    public partial class HttpParser
    {
        public HttpParser()
        {

        }


        public static HttpRequest ParseRequest(byte[] data)
        {
            HttpRequest result = new HttpRequest();
            int i = 0, icount = data == null ? 0 : data.Length;
            byte b, prevb = 0;
            int paramNum = 0, newLineNum = 0;
            StringBuilder current = new StringBuilder();

            Action nextParam = () =>
            {
                current.Clear();
                paramNum++;
            };
            Action setParam = () =>
            {
                string paramValue = current.ToString();
                if (!string.IsNullOrEmpty(paramValue))
                {
                    setRequestParam(result, paramNum, current.ToString());
                    nextParam();
                }
            };
            Action<byte> append = (bt) =>
            {
                newLineNum = 0;
                current.Append((char)bt);
            };

            while (i < icount)
            {
                b = data[i];
                if (newLineNum < 2)
                {
                    if (b == _space && paramNum < 3)
                    {
                        setParam();
                    }
                    else if (b == _enter)
                    {
                        prevb = b;
                    }
                    else if (b == _newline)
                    {
                        if (prevb == _enter)
                        {
                            setParam();
                            newLineNum++;
                        }
                    }
                    else append(b);
                }
                else append(b);
                i++;
            }
            result.Content = current.ToString();

            string accEnc = result.Parameters["Accept-Encoding"];
            if (!string.IsNullOrEmpty(accEnc))
            {
                result.SupportCompression += accEnc.Contains("gzip") ? (int)HttpCompress.Gzip : 0;
                result.SupportCompression += accEnc.Contains("deflate") ? (int)HttpCompress.Deflate : 0;
            }
            return result;
        }


        private static void setRequestParam(HttpRequest request, int paramNum, string param)
        {
            switch (paramNum)
            {
                case HttpHeader_Method  : request.Header.Method   = param; break;
                case HttpHeader_Path    : request.Header.Source   = param; parsePath(request); break;
                case HttpHeader_Protocol: request.Header.Protocol = param; break;
                default:
                    setRequestParam(request, param);
                    break;
            }
        }

        private static void parsePath(HttpRequest request)
        {
            if (!string.IsNullOrEmpty(request.Header.Source))
            {
                string[] values = request.Header.Source.Split('?');
                if (values != null)
                {
                    if (values.Length > 0)
                        parseSourceString(request, values[0]);
                        
                    if (values.Length > 1)
                        parseQueryString(request, values[1]);
                }
            }
        }

        private static void parseSourceString(HttpRequest request, string sourceString)
        {
            string value = sourceString;
            if (!string.IsNullOrEmpty(value))
            {
                int index = value.LastIndexOf('/');
                if (index > 0)
                {
                    request.Path = sourceString.Substring(0, index);
                    value = index + 1 < value.Length ? value.Substring(index + 1, value.Length - index - 1) : string.Empty;
                    if (!string.IsNullOrEmpty(value))
                        request.File.SetFile(value);
                }
                else request.Path = sourceString;
            }
        }


        private static void parseQueryString(HttpRequest request, string queryString)
        {
            if (!string.IsNullOrEmpty(queryString))
            {
                string[] values = queryString.Split('&');
                if (values != null)
                    for (int i = 0, icount = values.Length; i < icount; i++)
                        parseQueryStringValue(request, values[i]);
            }
        }

        private static void parseQueryStringValue(HttpRequest request, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string[] values = value.Split('=');
                if (values != null)
                {
                    request.QueryString.Add(values[0], values.Length > 1 ? values[1] : string.Empty);
                }
            }
        }

        private static void setRequestParam(HttpRequest request, string param)
        {
            if (!string.IsNullOrEmpty(param))
            {
                string[] paramNameValue = param.Split(':');
                string value = string.Empty;
                for (int i = 1, icount = paramNameValue.Length; i < icount; i++)
                    value = string.Concat(value, i > 1 ? ":" : string.Empty, paramNameValue[i].Trim());
                request.Parameters.Add(paramNameValue[0], value);
            }
        }
    }
}
