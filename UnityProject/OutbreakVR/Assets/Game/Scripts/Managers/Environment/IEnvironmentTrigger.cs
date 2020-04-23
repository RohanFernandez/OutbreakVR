using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{ 
    public interface IEnvironmentTrigger : IPointerOver
    {
        void onObjectHit();
    }
}