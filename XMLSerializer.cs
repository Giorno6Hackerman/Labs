using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Soap;
using System.Reflection;

namespace CRUD
{
    public class XMLSerializer : ISatanSerializer
    {

        //XmlSerializer xmlFormatter;
        SoapFormatter xmlFormatter;


        public XMLSerializer(/*Type type*/)
        {
            //Array arr = Array.CreateInstance(type, 1);
            //Type t = arr.GetType();
            //xmlFormatter = new XmlSerializer(arr.GetType());
            //xmlFormatter = new XmlSerializer(type);
            xmlFormatter = new SoapFormatter();
            xmlFormatter.Binder = new CustomBinder();
        }


        public void Serialize(Stream data, object[] graph)
        {
            try
            {
                //MemoryStream serializationStream = data as MemoryStream;
                //xmlFormatter = new XmlSerializer(graph[0].GetType());
                xmlFormatter.Serialize(data, graph);
                //serializationStream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public object[] Deserialize(Stream data)
        {
            try
            {
                //MemoryStream serializationStream = data as MemoryStream;
                //xmlFormatter = new XmlSerializer(graph[0].GetType());
                object[] obj = (object[])xmlFormatter.Deserialize(data);
                //object[] obj = new object[1] { ob };
                //object[] obj = (object[])xmlFormatter.Deserialize(serializationStream);
                //serializationStream.Close();
                return obj;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public class CustomBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                Assembly currentasm = Assembly.LoadFrom("D:/prog/4 sem/OOTPISP/Labs/StoneLibrary/bin/Debug/StoneLibrary.dll");
                //Assembly currentasm = Assembly.GetExecutingAssembly();
                //string name = String.Format("{0}, {1}", typeName, currentasm.FullName);
                //return Type.GetType($"{currentasm.GetName().Name}.{typeName.Split('.')[1]}");
                //string name = "StoneOcean." + ;
                Type type = currentasm.GetType(typeName);
                return type;
            }
        }
    }
}
