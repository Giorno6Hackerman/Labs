using System.IO;

namespace CRUD
{
    public interface ISatanSerializer
    {
        void Serialize(Stream serializationStream, object graph);
        object Deserialize(Stream serializationStream);
    }
}
