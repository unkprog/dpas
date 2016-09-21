using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using dpas.Core.Extensions;

namespace dpas.Core.IO.Settings
{
    public class BaseSettings
    {
        public BaseSettings() : this("InternalSettings")
        {

        }

        public BaseSettings(string sectionName)
        {
            SectionName = sectionName;
            dictSettings = new Dictionary<string, string>();
        }

        protected Dictionary<string, string> dictSettings { get; private set; }
        public string SectionName { get; private set; }


        public void Read(string fullFileName)
        {
            if (File.Exists(fullFileName))
            {
                dictSettings.Clear();
                using (XmlReader vReader = XmlReader.Create(fullFileName)) // new XmlTextReader(fullFileName))
                {
                    //try
                    //{
                    while (vReader.Read())
                    {
                        if (vReader.NodeType == XmlNodeType.Element && vReader.Name == SectionName)
                            ReadSettings(vReader);
                    }
                    //}
                    //finally { vReader.Close(); }
                }
            }
        }

        private void ReadSettings(XmlReader aReader)
        {
            string key;
            while (aReader.Read() && !(aReader.Name == SectionName && aReader.NodeType == XmlNodeType.EndElement))
            {
                if (aReader.NodeType == XmlNodeType.Element && aReader.Name.ToLower() == "add")
                {
                    key = aReader.GetString("key");
                    if (dictSettings.ContainsKey(key))
                        dictSettings[key] = aReader.GetString("value");
                    else
                        dictSettings.Add(key, aReader.GetString("value"));
                }
            }
        }

        public void Save(string fullFileName)
        {
            XElement xSettings = new XElement(SectionName);
            foreach (var keyValue in dictSettings)
                xSettings.Add(new XElement("add", new XAttribute("key", keyValue.Key), new XAttribute("value", keyValue.Value)));
            XDocument xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), xSettings);
            using (StreamWriter writer = File.CreateText(fullFileName))
            {
                xdoc.Save(writer); //.Save(fullFileName);
            }
        }

    }
}
