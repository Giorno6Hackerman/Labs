using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using StoneOcean;

namespace CRUD
{
    public class BinarySerializer : ISatanSerializer
    {
        BinaryFormatter formatter = new BinaryFormatter();
        public string libName;

        public BinarySerializer()
        {

        }

        public void Serialize(Stream serializationStream, object graph) 
        {
            
            formatter.Serialize(serializationStream, graph);
        }


        public object Deserialize(Stream serializationStream)
        {
            return formatter.Deserialize(serializationStream);
        }
    }
}
