using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace StoneOcean
{
    [Serializable]
    public class Class1 : ISerializable
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
    }
}
