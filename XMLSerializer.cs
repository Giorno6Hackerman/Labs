using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Soap;

namespace CRUD
{
    public class XMLSerializer : ISatanSerializer
    {

        XmlSerializer xmlFormatter;
        //SoapFormatter xmlFormatter;

        public XMLSerializer(Type type)
        {
            //Array arr = Array.CreateInstance(type, 1);
            //Type t = arr.GetType();
            //xmlFormatter = new XmlSerializer(arr.GetType());
            xmlFormatter = new XmlSerializer(type);
            
        }


        public void Serialize(Stream serializationStream, object[] graph)
        {
            try
            {
                xmlFormatter = new XmlSerializer(graph[0].GetType());
                xmlFormatter.Serialize(serializationStream, graph[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public object[] Deserialize(Stream serializationStream)
        {
            try
            {
                //xmlFormatter = new XmlSerializer(graph[0].GetType());
                object ob = xmlFormatter.Deserialize(serializationStream);
                object[] obj = new object[1] { ob };
                //object[] obj = (object[])xmlFormatter.Deserialize(serializationStream);
                return obj;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
