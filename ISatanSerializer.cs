using System.Collections.Generic;
using System.IO;

namespace CRUD
{
    public interface ISatanSerializer
    {
        void Serialize(Stream data, object[] graph);
        object[] Deserialize(Stream data);
    }
}
