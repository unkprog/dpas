using System;
using dpas.Core.Helpers;

namespace dpas.Core.Extensions
{
    public static class XmlReaderExt
    {
        public static string GetString(this System.Xml.XmlReader aReader, string aAttribute, string aDefault = "")
        {
            return TryParse.String(aReader.GetAttribute(aAttribute), aDefault);
        }

        public static bool GetBool(this System.Xml.XmlReader aReader, string aAttribute, bool aDefault = false)
        {
            return TryParse.Bool(aReader.GetAttribute(aAttribute), aDefault);
        }

        public static int GetInt32(this System.Xml.XmlReader aReader, string aAttribute, int aDefault = 0)
        {
            return TryParse.Int32(aReader.GetAttribute(aAttribute), aDefault);
        }

        public static double GetDouble(this System.Xml.XmlReader aReader, string aAttribute, double aDefault = 0.0)
        {
            return TryParse.Double(aReader.GetAttribute(aAttribute), aDefault);
        }

        public static DateTime GetDateTime(this System.Xml.XmlReader aReader, string aAttribute)
        {
            return TryParse.DateTime(aReader.GetAttribute(aAttribute));
        }

        public static DateTime GetDateTime(this System.Xml.XmlReader aReader, string aAttribute, DateTime aDefault)
        {
            return TryParse.DateTime(aReader.GetAttribute(aAttribute), aDefault);
        }
    }
}
