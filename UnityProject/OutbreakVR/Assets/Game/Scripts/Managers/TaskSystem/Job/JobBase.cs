using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public abstract class JobBase : IJob
    {
        public virtual void onDestroy()
        {

        }

        public virtual void onExecute()
        {

        }
    }
}