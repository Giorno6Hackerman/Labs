using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StoneOcean
{
    [Serializable]
    public class Class1 : ISerializable//, IXmlSerializable
    {
        public Class1()
        {

        }
        
        public int number;


        public Class1(SerializationInfo info, StreamingContext context)
        {
            number = (int)info.GetValue("number", typeof(int));
        }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("number", number);
        }

        /*
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteValue(number);    
        }


        public void ReadXml(XmlReader reader)
        {
            number = reader.ReadContentAsInt();
        }


        public XmlSchema GetSchema()
        {
            return null;
        }
        */

    }
}
