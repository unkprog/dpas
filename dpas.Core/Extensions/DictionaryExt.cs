using System;
using System.Collections.Generic;
using dpas.Core.Helpers;

namespace dpas.Core.Extensions
{
    public static class DictionaryStringObjectExt
    {
        public static object GetValue(this Dictionary<string, object> aValues, string aParam)
        {
            object result = null;
            if (!aValues.TryGetValue(aParam, out result))
                result = null;
            return result;
        }

        public static string GetString(this Dictionary<string, object> aValues, string aParam, string aDefault = "")
        {
            return TryParse.String(aValues.GetValue(aParam), aDefault);
        }

        public static int GetInt32(this Dictionary<string, object> aValues, string aParam, int aDefault = 0)
        {
            return TryParse.Int32(aValues.GetValue(aParam), aDefault);
        }

        public static bool GetBool(this Dictionary<string, object> aValues, string aParam, bool aDefault = false)
        {
            return TryParse.Bool(aValues.GetValue(aParam), aDefault);
        }

        public static DateTime GetDateTime(this Dictionary<string, object> aValues, string aParam)
        {
            return GetDateTime(aValues, aParam, TryParse.MinDate);
        }

        public static DateTime GetDateTime(this Dictionary<string, object> aValues, string aParam, DateTime aDefault)
        {
            return TryParse.DateTime(aValues.GetValue(aParam));
        }
    }

    public static class DictionaryStringStringExt
    {
        public static string GetString(this Dictionary<string, string> aValues, string aParam, string aDefault = "")
        {
            string result = aDefault;
            if (aValues.TryGetValue(aParam, out result))
                return TryParse.String(result, aDefault);
            else return aDefault;
        }

        public static int GetInt32(this Dictionary<string, string> aValues, string aParam, int aDefault = 0)
        {
            return TryParse.Int32(aValues.GetString(aParam), aDefault);
        }

        public static bool GetBool(this Dictionary<string, string> aValues, string aParam, bool aDefault = false)
        {
            return TryParse.Bool(aValues.GetString(aParam), aDefault);
        }

        public static DateTime GetDateTime(this Dictionary<string, string> aValues, string aParam)
        {
            return GetDateTime(aValues, aParam, TryParse.MinDate);
        }

        public static DateTime GetDateTime(this Dictionary<string, string> aValues, string aParam, DateTime aDefault)
        {
            return TryParse.DateTime(aValues.GetString(aParam));
        }
    }
}
