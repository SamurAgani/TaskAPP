using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Services.Abstract
{
    public interface ISamurSoft
    {
        string Serialize(object graph);
        object Deserialize(string des);

    }
}
