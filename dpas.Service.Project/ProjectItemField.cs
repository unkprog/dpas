using System;
using System.Collections.Generic;
using System.Xml;

namespace dpas.Service.Project
{
    public class ProjectItemField : ProjectItem, IProjectItemField
    {
        public ProjectItemField(object aOwner) : base(aOwner)
        {
        }

        public string TypeClass { get; internal set; }


        #region IReaderXml

        protected override void ReadProperties(XmlReader aReader)
        {
            base.ReadProperties(aReader);
            TypeClass = aReader.GetAttribute("TypeClass");
        }
        #endregion


        #region IWriterXml

        protected override void WriteProperties(XmlWriter aWriter)
        {
            base.WriteProperties(aWriter);
            aWriter.WriteAttributeString("TypeClass", TypeClass);
        }
       
        #endregion
    }
}
