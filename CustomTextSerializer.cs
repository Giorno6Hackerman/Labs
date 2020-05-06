using System.IO;

namespace CRUD
{
    public class CustomTextSerializer : ISatanSerializer
    {
        public void Serialize(Stream serializationStream, object graph)
        {

        }


        public object Deserialize(Stream serializationStream)
        {
            return null;
        }
    }
}
