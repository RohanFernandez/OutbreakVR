using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface IJob
    {
        void onExecute();
        void onComplete();
    }
}
