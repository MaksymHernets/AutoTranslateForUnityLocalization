using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodTime.Tools.FactoryTranslate
{
    public interface BaseServiceApi
    {
        bool CheckService();
        string GetNameService();
    }
}