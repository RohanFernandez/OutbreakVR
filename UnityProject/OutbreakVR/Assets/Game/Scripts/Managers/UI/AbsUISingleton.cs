using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class AbsUISingleton : AbsUIPanel, IComponentHandler
    {
        public abstract void destroy();
        public abstract void initialize();
    }
}