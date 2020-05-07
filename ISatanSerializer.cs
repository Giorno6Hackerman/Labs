using System.Collections.Generic;
using System.IO;

namespace CRUD
{
    public interface ISatanSerializer
    {
        void Serialize(string fileName, object[] graph);
        object[] Deserialize(string fileName);
    }
}
