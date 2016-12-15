using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace dpas.Core.IO
{
    public interface IReaderXml
    {
        void Read(XmlReader Reader);
    }


    public interface IWriterXml
    {
        void Write(XmlWriter Writer);
    }
}
