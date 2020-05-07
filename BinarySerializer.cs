using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace CRUD
{
    public class BinarySerializer : ISatanSerializer
    {
        BinaryFormatter formatter;

        public BinarySerializer()
        {
            formatter = new BinaryFormatter();
            formatter.Binder = new CustomBinder();
            
        }

        public void Serialize(string fileName, object[] graph) 
        {
            FileStream serializationStream = new FileStream(fileName, FileMode.OpenOrCreate);
            formatter.Serialize(serializationStream, graph);
            serializationStream.Close();
        }


        public object[] Deserialize(string fileName)
        {
            try
            {
                FileStream serializationStream = new FileStream(fileName, FileMode.OpenOrCreate);
                object[] obj = (object[])formatter.Deserialize(serializationStream);
                serializationStream.Close();
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
