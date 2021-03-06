﻿using dpas.Core.Helpers;
using System;
using System.Text;

namespace dpas.Net.Http
{
    public static partial class HttpParser
    {
        public static HttpRequest ParseRequest(byte[] data)
        {
            HttpRequest result = new HttpRequest();
            int i = 0, icount = data == null ? 0 : data.Length, startIndex = 0;
            byte b, prevb = 0;
            int paramNum = 0, newLineNum = 0;
            StringBuilder current = new StringBuilder();

            Action<int, int> setParam = (index, count) =>
            {
                if (count > 0)
                {
                    string paramValue = Encoding.UTF8.GetString(data, startIndex, count);
                    if (!string.IsNullOrEmpty(paramValue))
                    {
                        setRequestParam(result, paramNum, paramValue);
                        paramNum++; 
                    }
                }
                startIndex = index;
            };

            while (i < icount && newLineNum < 2)
            {
                b = data[i];
                if (b == _space && paramNum < 3)
                {
                    setParam(i + 1, i - startIndex);
                }
                else if (b == _enter)
                {
                    prevb = b;
                }
                else if (b == _newline)
                {
                    if (prevb == _enter)
                    {
                        setParam(i + 1, i - startIndex - 1);
                        newLineNum++;
                    }
                }
                else
                    newLineNum = 0;
                i++;
            }

            string accEnc;
            if (result.Parameters.TryGetValue(HttpHeader.AcceptEncoding, out accEnc) && !string.IsNullOrEmpty(accEnc))
            {
                result.SupportCompression += accEnc.Contains("gzip") ? (int)HttpCompress.Gzip : 0;
                result.SupportCompression += accEnc.Contains("deflate") ? (int)HttpCompress.Deflate : 0;
            }

            //result.ContentStream = string.Empty;
            ParseContent(result, data, startIndex, icount - startIndex);
            return result;
        }

        public static HttpRequest ParseContent(HttpRequest request, byte[] data, int index, int count)
        {
            if (count > 0)
                request.ContentStream.Write(data, index, count);
            return request;
        }

        private static void setRequestParam(HttpRequest request, int paramNum, string param)
        {
            switch (paramNum)
            {
                case HttpHeader_Method: request.Header.Method = param; break;
                case HttpHeader_Path: request.Header.Source = param; parsePath(request); break;
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
                        request.QueryString = parseQueryString(values[1]);
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


        public static IHttpQueryString parseQueryString(string queryString)
        {
            IHttpQueryString result = new HttpQueryString();
            if (!string.IsNullOrEmpty(queryString))
            {
                string[] values = queryString.Split('&');
                for (int i = 0, icount = values.Length; i < icount; i++)
                    parseQueryStringValue(result, values[i]);
            }
            return result;
        }

        private static void parseQueryStringValue(IHttpQueryString queryString, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string[] values = value.Split('=');
                queryString.Add(values[0], values.Length > 1 ? values[1] : string.Empty);
            }
        }

        private static void setRequestParam(HttpRequest request, string param)
        {
            if (!string.IsNullOrEmpty(param))
            {
                string[] paramNameValue = param.Split(':');
                string value = string.Empty;
                if (paramNameValue[0] == HttpHeader.Cookie)
                    request.Cookies = ParseCookie(paramNameValue[1]);
                else if (paramNameValue[0] == HttpHeader.ContentLength)
                {
                    request.Parameters.Add(paramNameValue[0], paramNameValue[1]);
                    request.ContentLength = TryParse.Int32(paramNameValue[1]);
                }
                else if (paramNameValue[0] == HttpHeader.Connection)
                {
                    request.Parameters.Add(paramNameValue[0], paramNameValue[1]);
                    request.ShouldKeepAlive = paramNameValue[1] == HttpHeader.ConnectionKeepAlive;
                }
                else
                {
                    for (int i = 1, icount = paramNameValue.Length; i < icount; i++)
                        value = string.Concat(value, i > 1 ? ":" : string.Empty, paramNameValue[i].Trim());
                    request.Parameters.Add(paramNameValue[0], value);
                }
            }
        }

        public static IHttpCookies ParseCookie(string cookieString)
        {
            IHttpCookies result = new HttpCookies();
            if (!string.IsNullOrEmpty(cookieString))
            {
                string[] values = cookieString.Split(';');
                for (int i = 0, icount = values.Length; i < icount; i++)
                    parseCookieStringValue(result, values[i]);
            }
            return result;
        }

        private static void parseCookieStringValue(IHttpCookies cookies, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string[] values = value.Split('=');
                string key = values[0].Trim();
                if (!cookies.ContainsKey(key))
                    cookies.Add(key, values.Length > 1 ? values[1] : string.Empty);
            }
        }

        public static IHttpFormParameters ParseFormParameters(string formParametersString)
        {
            IHttpFormParameters result = new HttpFormParameters();
            if (!string.IsNullOrEmpty(formParametersString))
            {
                string[] values = formParametersString.Split('&');
                for (int i = 0, icount = values.Length; i < icount; i++)
                    parseFormParametersStringValue(result, values[i]);
            }
            return result;
        }

        private static void parseFormParametersStringValue(IHttpFormParameters formParameters, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string[] values = value.Split('=');
                string key = values[0].Trim();
                if (!formParameters.ContainsKey(key))
                    formParameters.Add(key, values.Length > 1 ? values[1] : string.Empty);
            }
        }
    }
}
